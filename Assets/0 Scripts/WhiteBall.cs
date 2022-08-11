using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBall : Ball {
    public void ApplyForce(Vector3 force ) => _rigidbody.AddForce(force, ForceMode.Impulse);
}