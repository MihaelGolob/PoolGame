using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class Cue : MonoBehaviour {
    // inspector assigned
    [SerializeField] private WhiteBall _whiteBall;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private Slider _powerSlider;
    [SerializeField] private float _maxDistance = 0.2f;
    [SerializeField] private float _maxForce = 40f;

    // private variables
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    
    private Transform _meshTransform;
    private Vector3 _startPosition;
    private bool _lockRotation = false;
    
    private Vector3 _direction = Vector3.forward;
    
    private void Start() {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _collider = GetComponentInChildren<Collider>();

        _meshTransform = transform.GetChild(0).transform;
        _startPosition = _meshTransform.localPosition;
    }
    
    private void Update() {
        // keep the cue at the white ball
        transform.position = _whiteBall.transform.position;
        // rotate cue
        RotateToMouse();
        // set power
        var pos = _meshTransform.localPosition;
        _meshTransform.localPosition = new Vector3(pos.x, pos.y, _startPosition.z - (_maxDistance - _maxDistance * _powerSlider.value));
    }

    private void FixedUpdate() {
        // check if white ball is still
        if (_whiteBall.Rigidbody.velocity.magnitude < 0.01f) {
            _whiteBall.Rigidbody.velocity = Vector3.zero;
            EnableCue();
        }
        else DisableCue();
    }

    private void RotateToMouse() {
        // rotate only when button is down
        if (!Input.GetKey(KeyCode.Mouse0) || _lockRotation) return;
        
        var mousePos = Input.mousePosition;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseWorldPos.y = 1;

        // get direction from white ball to mouse
        _direction = (mouseWorldPos - _whiteBall.transform.position).normalized;
        // rotate towards mouse
        var angle = Vector3.SignedAngle(transform.forward, _direction, Vector3.down);
        var rot = new Vector3(0, -angle, 0);
        transform.Rotate(Time.deltaTime * _rotationSpeed * rot, Space.Self);
    }

    private void ApplyForceToWhiteBall() {
        // apply force to white ball
        _whiteBall.Rigidbody.AddForce(_direction * (1 - _powerSlider.value) * _maxForce, ForceMode.Impulse);
        // disable cue
        DisableCue();
    }

    private void EnableCue() {
        _meshRenderer.enabled = true;
    }

    private void DisableCue() {
        _meshRenderer.enabled = false;
    }
    
    // public methods
    public void LockRotation() {
        _lockRotation = true;
    }
    
    public void UnlockRotation() {
        _lockRotation = false;
        if (_powerSlider.value < 1)
            ApplyForceToWhiteBall();
    }
}