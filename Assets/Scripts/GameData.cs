using System;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "GameData")]
public class GameData : ScriptableObject
{
    public float speedChangeRate;
    public event Action<float> OnSpeedChange;
    public float Speed {
        get { return _speed; }
        set
        {
            _speed = value;
            OnSpeedChange?.Invoke(_speed);
        }
    }


    private float _speed;
}