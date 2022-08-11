using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public enum GameState {
    Start,
    PlayerTurn,
    EnemyTurn,
    GameOver
}

public class TurnManager : MonoBehaviour {
    // Singleton
    private static TurnManager _instance;

    public static TurnManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<TurnManager>();
            }

            return _instance;
        }
    }
    
    // inspector assigned
    [SerializeField] private Unit _player;
    [SerializeField] private Unit _enemy;
    [SerializeField] private float _checkBallStillInterval = 0.5f;
    [SerializeField] private List<Ball> _balls;
    
    // events
    public event Action OnGameStart;
    public event Action<Unit> OnChangeTurn;

    // private variables
    private GameState _gameState = GameState.Start;
    private BallType _playerBallType = BallType.None;
    private BallType _enemyBallType = BallType.None;
    private Unit _onTurn;
    
    private float _checkBallStillTimer;

    private void OnEnable() {
        // subscribe to events
        Ball.OnBallPocketed += HandleOnBallPocketed;
    }

    private void OnDisable() {
        // unsubscribe from events
        Ball.OnBallPocketed -= HandleOnBallPocketed;
    }

    private void Start() {
        // initialization
        ChangeTurn(_player);
        OnGameStart?.Invoke();
    }

    private void FixedUpdate() {
        _checkBallStillTimer += Time.deltaTime;
        
        // dont check every frame
        if (_checkBallStillTimer < _checkBallStillInterval) return;
        
        _checkBallStillTimer = 0f;
        if (CheckBallsStill() && _onTurn.HasShot) 
            ChangeTurn();
    }

    // private methods
    private void ChangeTurn(Unit unit) {
        _gameState = unit == _player ? GameState.PlayerTurn : GameState.EnemyTurn;
        _onTurn = unit;
        OnChangeTurn?.Invoke(unit);
    }

    private void ChangeTurn() => ChangeTurn(_gameState == GameState.PlayerTurn ? _enemy : _player);

    // check if all the balls are still
    //private bool CheckBallsStill() => _balls.All(ball => ball.IsStill);
    private bool CheckBallsStill() {
        foreach (var ball in _balls) {
            if (!ball.IsStill) return false;
        }

        return true;
    }

    // event handler methods
    private void HandleOnBallPocketed(Ball ball) {
        // TODO make necessary actions 
    }
    
    // public methods
    public Cue GetActiveCue() => _onTurn.Cue;
}