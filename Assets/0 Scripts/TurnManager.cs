using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState {
    Start,
    PlayerBreaks,
    BallInHand,
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
    public event Action<Unit> OnChangeTurn;
    
    // public properties
    public Cue GetActiveCue => _onTurn.Cue;

    // private variables
    private GameState _gameState = GameState.Start;
    private BallType _playerBallType = BallType.None;
    private BallType _enemyBallType = BallType.None;
    private Unit _onTurn;

    private bool _shotThisTurn;

    private int _ballsPocketedThisRound;
    private List<Ball> _ballsPocketed = new(16);

    private float _checkBallStillTimer;
     
    // ---------------------------------------------------------------------------------------------
    // UNITY EVENT METHODS
    
    // subscribe to events
    private void OnEnable() {
        Ball.OnBallPocketed += HandleOnBallPocketed;
        Cue.OnShot += HandleOnShot;
    }

    private void OnDisable() {
        Ball.OnBallPocketed -= HandleOnBallPocketed;
    }

    private void Start() {
        // initialization
        ChangeTurn(_player);
        // we dont need start state at the moment
        _gameState = GameState.PlayerBreaks;
    }

    private void Update() {
        
    }

    private void FixedUpdate() {
        _checkBallStillTimer += Time.deltaTime;
        
        // dont check every frame
        if (_checkBallStillTimer < _checkBallStillInterval) return;
        
        _checkBallStillTimer = 0f;
        if (CheckBallsStill() && _shotThisTurn) 
            ChangeTurn();
    }
    
    // ---------------------------------------------------------------------------------------------

    // ---------------------------------------------------------------------------------------------
    // PRIVATE METHODS
    private void ChangeTurn(Unit unit) {
        _gameState = unit == _player ? GameState.PlayerTurn : GameState.EnemyTurn;
        _onTurn = unit;
        OnChangeTurn?.Invoke(unit);
        
        // reset states
        _shotThisTurn = false;
    }

    private void ChangeTurn() => ChangeTurn(_gameState == GameState.PlayerTurn ? _enemy : _player);

    // check if all the balls are still
    private bool CheckBallsStill() => _balls.All(ball => ball.IsStill);
    // private bool CheckBallsStill() {
    //     foreach (var ball in _balls) {
    //         if (!ball.IsStill) {
    //             var neki = ball.IsStill;
    //             return false;
    //         }
    //     }
    //     return true;
    // }

    // ---------------------------------------------------------------------------------------------
    // EVENT HANDLERS
    private void HandleOnBallPocketed(Ball ball) {
        
    }
    
    private void HandleOnShot() {
        _shotThisTurn = true;
    }
    
}