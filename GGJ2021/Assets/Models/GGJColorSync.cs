using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class GGJColorSync: RealtimeComponent<GGJColorSyncModel> {
    
    private MeshRenderer _meshRenderer;

    private void Awake() {
        // Get a reference to the mesh renderer
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    /// Register for an event from the model that will fire whenever it’s changed locally or by a remote client.
    protected override void OnRealtimeModelReplaced(GGJColorSyncModel previousModel, GGJColorSyncModel currentModel) {
        if (previousModel != null) {
            // unregister from events
            previousModel.colorDidChange -= ColorDidChange;
        }

        if (currentModel != null) {
            // if this is a model that has no data set on it, populate it with the current mesh renderer color
            if (currentModel.isFreshModel)
                currentModel.color = _meshRenderer.material.color;

            // Update the mesh renderer to match the new model
            UpdateMeshRendererColor();

            // Register for events so we'll know if the color changes later
            currentModel.colorDidChange += ColorDidChange;
        }
    }

    private void ColorDidChange(GGJColorSyncModel model, Color value) {
        // Update the mesh renderer
        UpdateMeshRendererColor();
    }

    private void UpdateMeshRendererColor() {
        // Get the color from the model and set it on the mesh renderer.
        _meshRenderer.material.color = model.color;
    }

    public void SetColor(Color color) {
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        model.color = color;
    }
}
