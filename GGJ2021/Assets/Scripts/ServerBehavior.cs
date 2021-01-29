using UnityEngine;
using UnityEngine.Assertions;

using Unity.Collections;
using Unity.Networking.Transport;

public class ServerBehavior: MonoBehaviour {
    
    public NetworkDriver m_Driver;
    private NativeList<NetworkConnection> m_Connections;

    void Start() {
        // create our driver
        m_Driver = NetworkDriver.Create();
        
        // Then we try to bind our driver to a specific network address and port, and if that does not fail, we call the Listen method.
        var endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = 9000;
        if (m_Driver.Bind(endpoint) != 0) {
            Debug.Log("Failed to bind to port 9000");
        } else {
            m_Driver.Listen();
        }

        // finally create the NativeList to hold all connections
        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
    }

    /** 
        Both NetworkDriver and NativeList allocate unmanaged memory and need to be disposed. 
        To make sure this happens we can simply call the Dispose method when we are done with both of them.
    */
    public void OnDestroy() {
        m_Driver.Dispose();
        m_Connections.Dispose();
    }

    /**
    ## Server Update Loop

    As the com.unity.transport package uses the Unity C# Job System internally, the m_Driver has a ScheduleUpdate method call. 
    Inside our Update loop you need to make sure to call the Complete method on the JobHandle that is returned, in order to 
    know when you are ready to process any updates.
    */
    void Update() {
        m_Driver.ScheduleUpdate().Complete();

        // The first thing we want to do, after you have updated your m_Driver, is to handle your connections. 
        // Start by cleaning up any old stale connections from the list before processing any new ones. 
        // This cleanup ensures that, when we iterate through the list to check what new events we have gotten, 
        // we dont have any old connections laying around.
        for (int i = 0; i < m_Connections.Length; i++) {
            //  we iterate through our connection list and just simply remove any stale connections.
            if (!m_Connections[i].IsCreated) {
                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        // Accept new connections
        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default(NetworkConnection)) {
            m_Connections.Add(c);
            Debug.Log("Accepted a connection");
        }

        // Now we have an up-to-date connection list. 
        // You can now start querying the driver for events that might have happened since the last update. 
        DataStreamReader stream;
        for (int i = 0; i < m_Connections.Length; i++) {

            Assert.IsTrue( m_Connections[i].IsCreated );

            // For each connection we want to call PopEventForConnection while there are more events still needing to get processed.
            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty) {

                // ready to process events!

                // lets start with the Data Event
                if (cmd == NetworkEvent.Type.Data) {

                    // try to read an uint from the stream and output what we received
                    uint number = stream.ReadUInt();
                    Debug.Log("Got " + number + " from the Client adding + 2 to it.");

                    // When this is done we simply add two to the number we received and send it back.
                    number +=2;

                    // To send anything with the NetworkDriver we need a instance of a DataStreamWriter.
                    // You get a DataStreamWriter when you start sending a message by calling BeginSend.
                    //
                    // Note that we are sending NetworkPipeline.Null to the BeginSend function. 
                    // This way we say to the driver to use the unreliable pipeline to send our data. 
                    // It is also possible to not specify a pipeline.
                    var writer = m_Driver.BeginSend(NetworkPipeline.Null, m_Connections[i]);
                    writer.WriteUInt(number);
                    m_Driver.EndSend(writer);
                }

                // Finally, handle the disconnect case.
                else if (cmd == NetworkEvent.Type.Disconnect) {
                    // This is pretty straight forward, if you receive a disconnect message you need to reset 
                    // that connection to a default(NetworkConnection). 
                    // As you might remember, the next time the Update loop runs you will clean up after yourself.
                    Debug.Log("Client disconnected from server");
                    m_Connections[i] = default(NetworkConnection);
                }
            }
        }
    }
}
