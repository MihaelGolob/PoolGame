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
    [Header("Notifications")]
    [SerializeField] private TMP_Text _notificationText;

    // private variables
    private Unit _player1;
    private Unit _player2;
    
    // subscribe to events
    private void OnEnable() {
        TurnManager.Instance.OnChangeTurn += HandleOnChangeTurn;
        TurnManager.Instance.OnBallTypesSet += HandleOnBallTypesSet;
        TurnManager.Instance.OnGameOver += HandleOnGameOver;
    }
    
    // unsubscribe from events
    private void OnDisable() {
        TurnManager.Instance.OnChangeTurn -= HandleOnChangeTurn;
        TurnManager.Instance.OnBallTypesSet -= HandleOnBallTypesSet;
        TurnManager.Instance.OnGameOver -= HandleOnGameOver;
    }

    private void Start() {
        // player text
        _player1 = TurnManager.Instance.Player1;
        _player2 = TurnManager.Instance.Player2;
        
        _player1Text.text = _player1.Name;
        _player2Text.text = _player2.Name;
        
        _player1Text.color = _offTurnColor;
        _player2Text.color = _offTurnColor;
        
        // other
        _notificationText.text = "";
    }
    
    private IEnumerator Notification(string text, float duration) {
        _notificationText.text = text;
        yield return new WaitForSeconds(duration);
        _notificationText.text = "";
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
    
    private void HandleOnBallTypesSet() {
        var type = TurnManager.Instance.ActiveUnit.BallType == BallType.Solids ? "solids" : "stripes";
        StartCoroutine(Notification($"You are {type}!", 5f));
    }

    private void HandleOnGameOver(Unit unit) { 
        StartCoroutine(Notification($"{unit.Name} wins!", 5f));
    }
}