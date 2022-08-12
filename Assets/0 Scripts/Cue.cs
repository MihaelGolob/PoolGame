using System;
using System.Collections;
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
    
    private Vector3 _aimDirection = Vector3.forward;
    
    // public fields
    public Vector3 AimDirection => _aimDirection;
    public bool IsCueDisabled { get; set; }

    // events
    public static event Action OnShot;
    
    private void Start() {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _collider = GetComponentInChildren<Collider>();

        _meshTransform = transform.GetChild(0).transform;
        _startPosition = _meshTransform.localPosition;
    }
    
    private void Update() {
        // keep the cue at the white ball
        var whitePos = _whiteBall.transform.position;
        transform.position = new Vector3(whitePos.x, transform.position.y, whitePos.z);
        // rotate cue
        RotateToMouse();
        // set power
        var pos = _meshTransform.localPosition;
        _meshTransform.localPosition = new Vector3(pos.x, pos.y, _startPosition.z - (_maxDistance - _maxDistance * _powerSlider.value));
    }

    private void RotateToMouse() {
        // rotate only when button is down
        if (!Input.GetKey(KeyCode.Mouse0) || _lockRotation || IsCueDisabled) return;
        
        var mousePos = Input.mousePosition;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseWorldPos.y = 1;

        // get direction from white ball to mouse
        _aimDirection = (mouseWorldPos - _whiteBall.transform.position).normalized;
        // rotate towards mouse
        var angle = Vector3.SignedAngle(transform.forward, _aimDirection, Vector3.down);
        var rot = new Vector3(0, -angle, 0);
        transform.Rotate(Time.deltaTime * _rotationSpeed * rot, Space.Self);
    }

    private void ApplyForceToWhiteBall() => _whiteBall.ApplyForce(_aimDirection * (1 - _powerSlider.value) * _maxForce);

    // public methods
    public void EnableCue() {
        IsCueDisabled = false;
        _lockRotation = false;
        if (_meshRenderer) _meshRenderer.enabled = true;
    }

    public void DisableCue() {
        IsCueDisabled = true;
        _lockRotation = true;
        if (_meshRenderer) _meshRenderer.enabled = false;
    }
    
    public void LockRotation() {
        _lockRotation = true;
    }
    
    public void UnlockRotation() {
        if (IsCueDisabled) return;
        _lockRotation = false;

        if (_powerSlider.value >= 1) return;
        StartCoroutine(UnlockRotationInternal());
    }

    private IEnumerator UnlockRotationInternal() {
        ApplyForceToWhiteBall();
        DisableCue();
        // delay for the ball to gain velocity
        yield return new WaitForSeconds(0.1f);
        OnShot.Invoke();
    }
}