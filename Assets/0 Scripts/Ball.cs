using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType {
    Solids,
    Stripes,
    Black,
    White
}

public class Ball : MonoBehaviour {
    // inspector assigned
    [SerializeField] protected BallType _ballType;
    [SerializeField] protected float _gravityMultiplier = 3f;
 
    // private variables
    private event Action<Ball> OnBallPocketed;

    // public properties
    public BallType BallType => _ballType;
    public Rigidbody Rigidbody { get; private set; }

    protected virtual void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Start() {
        // apply extra gravity so the balls dont exit the table
        Rigidbody.AddForce(Vector3.down * _gravityMultiplier, ForceMode.Acceleration);
    }

    protected virtual void Update() {
        
    }

    protected void FixedUpdate() {
        if (Rigidbody.velocity.magnitude < 0.01f)
            Rigidbody.velocity = Vector3.zero;
    }

    protected void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Pocket")) return;
        
        // disable renderer
        GetComponent<Renderer>().enabled = false;
        
        OnBallPocketed?.Invoke(this);
    }
}