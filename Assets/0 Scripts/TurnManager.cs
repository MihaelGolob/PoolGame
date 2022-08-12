using System;
using System.Collections;
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
    public event Action OnBallTypesSet;
    public event Action<Unit> OnGameOver;

    // public properties
    public Unit ActiveUnit => _onTurn;
    public Unit Player1 => _player;
    public Unit Player2 => _enemy;

    // private variables
    private GameState _gameState = GameState.Start;
    private Unit _onTurn;

    private bool _shotThisTurn;
    
    private readonly List<Ball> _ballsPocketed = new(16);
    private readonly List<Ball> _ballsPocketedThisTurn = new();

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
        Cue.OnShot -= HandleOnShot;
    }

    private void Start() {
        // initialization
        ChangeTurn(_player);
        // we dont need start state at the moment
        _gameState = GameState.PlayerBreaks;
    }

    private void Update() {
        _checkBallStillTimer += Time.deltaTime;
        
        // dont check every frame
        if (_checkBallStillTimer < _checkBallStillInterval) return;
        
        _checkBallStillTimer = 0f;
        
        // check if units turn is over
        if (!CheckBallsStill() || !_shotThisTurn) return;
        
        // check if game is over
        var blackPocketed = _ballsPocketedThisTurn.Any(ball => ball.BallType == BallType.Black);
        var allOthersPocketed = _ballsPocketed.Count(ball => ball.BallType == _onTurn.BallType) == 7;
        if (blackPocketed) {
            if (allOthersPocketed) OnGameOver?.Invoke(_onTurn);
            else OnGameOver?.Invoke(NotOnTurn());
        } 
        
        // check ball types are set
        if (_player.BallType == BallType.None || _enemy.BallType == BallType.None) {
            SetBallTypes();
        }
            
        // change turn if no/wrong balls were pocketed, or else dont change turn
        var wrongBallsPocketed = _ballsPocketedThisTurn.Any(ball => ball.BallType != _onTurn.BallType);

        if (_ballsPocketedThisTurn.Count > 0 && (_onTurn.BallType == BallType.None || !wrongBallsPocketed))
            ChangeTurn(_onTurn);
        else 
            ChangeTurn();
    }
    
    // ---------------------------------------------------------------------------------------------

    // ---------------------------------------------------------------------------------------------
    private void SetBallTypes() {
        if (_gameState == GameState.PlayerBreaks || _ballsPocketedThisTurn.Count <= 0) return;
        
        var ballType = _ballsPocketedThisTurn[0].BallType;
        var otherBallType = ballType == BallType.Solids ? BallType.Stripes : BallType.Solids;
        // set ball types
        _onTurn.BallType = ballType;
        NotOnTurn().BallType = otherBallType;

        OnBallTypesSet?.Invoke();
    }
    
    // PRIVATE METHODS
    private void ChangeTurn(Unit unit) {
        // reset states
        _shotThisTurn = false;
        _ballsPocketedThisTurn.Clear();
        // change states
        _gameState = unit == _player ? GameState.PlayerTurn : GameState.EnemyTurn;
        
        _onTurn = unit;
        OnChangeTurn?.Invoke(unit);
    }

    private void ChangeTurn() => ChangeTurn(_onTurn == _player ? _enemy : _player);

    // check if all the balls are still
    private bool CheckBallsStill() => _balls.All(ball => ball.IsStill);

    private int BallsOfTypePocketed(BallType type) => _ballsPocketed.Count(ball => ball.BallType == type);
    
    private Unit NotOnTurn() => _onTurn == _player ? _enemy : _player;

    private IEnumerator RespawnWhiteDelayed(Ball ball) {
        _onTurn.Cue.DisableCue();
        yield return new WaitForSeconds(4f);
        var whiteBall = ball.gameObject.GetComponent<WhiteBall>();
        // respawn white ball
        whiteBall.Respawn();
        _onTurn.Cue.EnableCue();
    }
    
    // ---------------------------------------------------------------------------------------------
    // EVENT HANDLERS
    private void HandleOnBallPocketed(Ball ball) {
        Debug.Log($"Ball pocketed: {ball.gameObject.name}");

        if (ball.BallType == BallType.White) {
            StartCoroutine(RespawnWhiteDelayed(ball));
            return;
        }
        
        _ballsPocketed.Add(ball);
        _ballsPocketedThisTurn.Add(ball);
    }
    
    private void HandleOnShot() {
        _shotThisTurn = true;
    }
    
}