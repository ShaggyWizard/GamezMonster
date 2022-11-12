using System;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "GameData")]
public class GameData : ScriptableObject
{
    public bool pause;
    public float time;
    public int difficulty;
    public event Action<float> OnSpeedChange;
    public float Speed {
        get { return pause ? 0f : _speed; }
        set
        {
            _speed = value;
            OnSpeedChange?.Invoke(_speed);
        }
    }


    private float _speed;
}