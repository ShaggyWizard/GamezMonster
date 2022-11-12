using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private GameData _gameData;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _playerSpawn;
    [SerializeField] private UnityEvent _onLose;


    private float _nextSpeedUp;
    private float _startTime;
    private ITrajectory _playerTrajectory;
    private IDeath _death;


    private void Start()
    {
        bool valid = true;

        _nextSpeedUp = _gameSettings.speedChangeRate;
        if (_playerPrefab == null)
        {
            Debug.LogError("Game - Player Prefab is null.");
            Destroy(this);
            return;
        }
        if (_playerSpawn == null)
        {
            Debug.LogError("Game - Player Spawn is null.");
            Destroy(this);
            return;
        }
        var player = Instantiate(_playerPrefab, transform);
        if (!player.TryGetComponent(out _playerTrajectory))
        {
            Debug.LogError("Game - Player Prefab must have a ITrajectory.");
            valid = false;
        }
        if (!player.TryGetComponent(out _death))
        {
            Debug.LogError("Game - Player Prefab must have a IDeath.");
            valid = false;
        }
        if (!valid)
        {
            Destroy(this);
        }
        _playerTrajectory.SetPosition(_playerSpawn.transform.position);
        _playerTrajectory.SetSpeed(_gameSettings.playerStartSpeed);

        _death.OnDeath += Lose;
        _startTime = Time.time;
        _gameData.pause = false;
    }

    private void Update()
    {
        if (Time.time >= _nextSpeedUp)
        {
            _nextSpeedUp += _gameSettings.speedChangeRate;
            _playerTrajectory.SetSpeed(_playerTrajectory.Velocity.magnitude + _gameSettings.speedChangeAmount);
        }
    }
    private void Lose()
    {
        _gameData.pause = true;
        _gameData.time = Time.time - _startTime;
        _onLose?.Invoke();
    }
}
