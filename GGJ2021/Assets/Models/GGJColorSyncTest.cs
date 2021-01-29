using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGJColorSyncTest: MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField]
    private Color _color = default;
    private Color _previousColor = default;

    private GGJColorSync _colorSync;
    
    private void Awake() {
        // Get a reference to the color sync component
        _colorSync = GetComponent<GGJColorSync>();
    }

    // Update is called once per frame
    private void Update() {
        // If the color has changed (via the inspector), call setColor on the Color Sync Component
        if (_color != _previousColor) {
            _colorSync.SetColor(_color);
            _previousColor = _color;
        }
    }
}
