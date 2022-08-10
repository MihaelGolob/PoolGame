using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AimLine : MonoBehaviour {
    // inspector assigned
    [SerializeField] private Transform _whiteBall;
    [SerializeField] private Cue _cue;
    [SerializeField] private float _bouncedLineLenghth = 0.2f;

    // private variables
    private LineRenderer _lineRenderer;
    
    private void Start() {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    
    private void Update() {
        var verticalOffset = Vector3.up * 0.05f;
        
        // draw line from cue to white ball
        _lineRenderer.SetPosition(0, _whiteBall.position + verticalOffset);
        // raycast in direction of cue
        if (Physics.Raycast(_whiteBall.position, _cue.AimDirection, out var hit)) {
            _lineRenderer.SetPosition(1, hit.point + verticalOffset);
            
            // check hit layer
            if (hit.collider.CompareTag("Ball")) {
                // draw ball direction
                var normal = hit.normal;
                _lineRenderer.SetPosition(2, hit.point - normal * _bouncedLineLenghth + verticalOffset);
            }
            else {
                _lineRenderer.SetPosition(2, hit.point + verticalOffset);
            }
        }
        
        // hide line, when cue is hidden
        _lineRenderer.enabled = !_cue.IsCueDisabled;
    }
}