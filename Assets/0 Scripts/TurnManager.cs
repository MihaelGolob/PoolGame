using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
    
    // events
    public event Action OnGameStart;
    public event Action<Unit> OnPlayerTurn;

    // private variables
    private GameState _gameState;
    private BallType _playerBallType = BallType.None;
    private BallType _enemyBallType = BallType.None;


    private void OnEnable() {
        // subscribe to events
        Ball.OnBallPocketed += HandleOnBallPocketed;
    }

    private void OnDisable() {
        // unsubscribe from events
        Ball.OnBallPocketed -= HandleOnBallPocketed;
    }

    private IEnumerator Start() {
        yield return new WaitForSeconds(3f);
        OnGameStart?.Invoke();
        OnPlayerTurn?.Invoke(_player);
        _gameState = GameState.PlayerTurn;
    }

    private void Update() {
    }

    // check if all the balls are still
    public bool CheckIfAllBallsAreStill() {
        return false;
    }

    // event handler methods
    private void HandleOnBallPocketed(Ball ball) {
        // TODO make necessary actions 
    }
}