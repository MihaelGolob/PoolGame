using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cue : MonoBehaviour {
    // inspector assigned
    [SerializeField] private WhiteBall _whiteBall;
    [SerializeField] private float _rotationSpeed = 10f;

    private void Start() {
        
    }
    
    private void Update() {
        // keep the cue at the white ball
        transform.position = _whiteBall.transform.position;
        // rotate cue
        RotateToMouse();
    }

    private void RotateToMouse() {
        // rotate only when button is down
        if (!Input.GetKey(KeyCode.Mouse0)) return;
        
        var mousePos = Input.mousePosition;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseWorldPos.y = 1;

        // get direction from white ball to mouse
        var direction = (mouseWorldPos - _whiteBall.transform.position).normalized;
        // rotate towards mouse
        var angle = Vector3.SignedAngle(transform.forward, direction, Vector3.down);
        var rot = new Vector3(0, -angle, 0);
        transform.Rotate(Time.deltaTime * _rotationSpeed * rot, Space.Self);

        Debug.Log(angle);
        
    }
}