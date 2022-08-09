using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerSlider : MonoBehaviour {
    // inspector assigned
    [SerializeField] private float _sliderSpeed = 2f;
    
    // internal variables
    private Slider _slider;
    private bool _lockSlider;
    
    private void Start() {
        _slider = GetComponent<Slider>();
    }

    private void Update() {
        // increase slider value
        if (_slider && !_lockSlider) _slider.value += Time.deltaTime * _sliderSpeed;
    }

    // public methods
    
    public void LockSlider() {
        _lockSlider = true;
    }
    
    public void UnlockSlider() {
        _lockSlider = false;
    }
}