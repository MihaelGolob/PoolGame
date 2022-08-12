using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    // inspector assigned
    [Header("Player text")]
    [SerializeField] private TMP_Text _player1Text;
    [SerializeField] private TMP_Text _player2Text;
    [SerializeField] private Color _offTurnColor = Color.white;
    [SerializeField] private Color _onTurnColor = Color.red;

    // private variables
    private Unit _player1;
    private Unit _player2;
    
    // subscribe to events
    private void OnEnable() {
        TurnManager.Instance.OnChangeTurn += HandleOnChangeTurn;
    }
    
    // unsubscribe from events
    private void OnDisable() {
        TurnManager.Instance.OnChangeTurn -= HandleOnChangeTurn;
    }

    private void Start() {
        _player1 = TurnManager.Instance.Player1;
        _player2 = TurnManager.Instance.Player2;
        
        _player1Text.text = _player1.Name;
        _player2Text.text = _player2.Name;
        
        _player1Text.color = _offTurnColor;
        _player2Text.color = _offTurnColor;
    }

    // event handlers
    private void HandleOnChangeTurn(Unit unit) {
        if (unit == _player1) {
            _player1Text.color = _onTurnColor;
            _player2Text.color = _offTurnColor;   
        }
        else {
            _player1Text.color = _offTurnColor;
            _player2Text.color = _onTurnColor;
        }
    }
}