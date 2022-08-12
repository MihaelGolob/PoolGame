using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    // inspector assigned
    [SerializeField] private string _unitName;
    [SerializeField] private Cue _cue;

    // public properties
    public Cue Cue => _cue;
    public BallType BallType { get; set; } = BallType.None;
    public string Name => _unitName;

    private void OnEnable() {
        TurnManager.Instance.OnChangeTurn += HandleOnChangeTurn;
    }
    
    private void OnDisable() {
        if (TurnManager.Instance) TurnManager.Instance.OnChangeTurn -= HandleOnChangeTurn;
    }
    
    // event handlers
    private void HandleOnChangeTurn(Unit unit) {
        if (unit != this) _cue.DisableCue();
        else _cue.EnableCue();
    }
}