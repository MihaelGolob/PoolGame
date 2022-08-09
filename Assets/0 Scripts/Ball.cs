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
 
    // private variables
    public event Action<Ball> OnBallPocketed;
    
    protected virtual void Start() {
        
    }
    
    protected virtual void Update() {
        
    }

    protected void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Pocket")) return;
        
        OnBallPocketed?.Invoke(this);
    }
}