using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType {
    Solids,
    Stripes,
    Black,
    White,
    None
}

public class Ball : MonoBehaviour {
    // inspector assigned
    [SerializeField] protected BallType _ballType;
    [SerializeField] protected float _gravityMultiplier = 3f;
    [SerializeField] protected float _stopBallTreeshold = 0.01f;
    
    // protected variables
    protected SphereCollider _collider;
    protected Rigidbody _rigidbody;
    protected bool _pocketed = false;

    // events
    public static event Action<Ball> OnBallPocketed;
    
    // public properties
    public BallType BallType => _ballType;
    public float Radius => _collider.radius;

    public bool IsStill => _pocketed || _rigidbody.velocity.magnitude <= _stopBallTreeshold;

    protected void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
    }

    protected virtual void Start() {
        // apply extra gravity so the balls dont exit the table
        _rigidbody.AddForce(Vector3.down * _gravityMultiplier, ForceMode.Acceleration);
    }

    protected void FixedUpdate() {
        // stop ball if very slow
        if (_rigidbody.velocity.magnitude < 0.01f)
            _rigidbody.velocity = Vector3.zero;
    }

    protected void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Pocket")) return;
        
        // disable renderer with delay
        _pocketed = true;
        StartCoroutine(DisableRenderer());
        _rigidbody.constraints = RigidbodyConstraints.None;
        // invoke the event
        OnBallPocketed?.Invoke(this);
        // prevent further event invocations
        _collider.enabled = false;
    }
    
    private IEnumerator DisableRenderer() {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Renderer>().enabled = false;
    }
}