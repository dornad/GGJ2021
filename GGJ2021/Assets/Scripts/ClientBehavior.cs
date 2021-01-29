using UnityEngine;
using Unity.Networking.Transport;

public class ClientBehavior : MonoBehaviour {

    public NetworkDriver m_Driver;
    public NetworkConnection m_Connection;
    public bool Done;
    
    void Start() {
        // Start by creating a driver for the client and an address for the server.
        m_Driver = NetworkDriver.Create();
        m_Connection = default(NetworkConnection);

        var endpoint = NetworkEndPoint.LoopbackIpv4;
        endpoint.Port = 9000;
        m_Connection = m_Driver.Connect(endpoint);
    }

    public void OnDestroy() {
        m_Driver.Dispose();
    }

    void Update() {
        // Start by calling m_Driver.ScheduleUpdate().Complete(); 
        // and make sure that the connection worked.
        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated) {
            if (!Done)
                Debug.Log("Something went wrong during connect");
            return;
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty) {

            // ready to process events!

            // lets start with the Connect Event
            if (cmd == NetworkEvent.Type.Connect) {
                Debug.Log("We are now connected to the server");

                // When you establish a connection between the client and the server, 
                // you send a number (that you want the server to increment by two). 
                // The use of the BeginSend / EndSend pattern together with the DataStreamWriter, 
                // where we set value to one, write it into the stream, and finally send it out on the network.
                uint value = 1;
                var writer = m_Driver.BeginSend(m_Connection);
                writer.WriteUInt(value);
                m_Driver.EndSend(writer);
            } 
            else if (cmd == NetworkEvent.Type.Data) {
                // When the NetworkEvent type is Data, read the value back that you received 
                // from the server and then call the Disconnect method.
                uint value = stream.ReadUInt();
                Debug.Log("Got the value = " + value + " back from the server");
                Done = true;
                m_Connection.Disconnect(m_Driver);
                m_Connection = default(NetworkConnection);
            }
            else if (cmd == NetworkEvent.Type.Disconnect) {
                Debug.Log("Client got disconnected from server");
                m_Connection = default(NetworkConnection);
            }
        }
    }
}
