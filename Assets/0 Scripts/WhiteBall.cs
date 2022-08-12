using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBall : Ball {
    // inspector assigned
    [SerializeField] private Transform _respawnPoint;
    
    public void ApplyForce(Vector3 force ) => _rigidbody.AddForce(force, ForceMode.Impulse);

    public void Respawn() {
        transform.position = _respawnPoint.position;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        
        _collider.enabled = true;
        _renderer.enabled = true;
    }
}