using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    // inspector assigned
    [SerializeField] private string _unitName;
    [SerializeField] private Cue _cue;
    
    // private variables
    private bool _isPlaying;
    
    // public properties
    public Cue Cue => _cue;

    private void OnEnable() {
        TurnManager.Instance.OnChangeTurn += HandleOnChangeTurn;
    }
    
    private void OnDisable() {
        if (TurnManager.Instance) TurnManager.Instance.OnChangeTurn -= HandleOnChangeTurn;
    }
    
    // event handlers
    private void HandleOnChangeTurn(Unit unit) {
        if (unit != this) {
            _isPlaying = false;
            _cue.DisableCue();
        }
        else {
            _isPlaying = true;
            _cue.EnableCue();
        }
    }
}