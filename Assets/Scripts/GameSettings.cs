using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    public float spacing;
    public List<float> difficulties;
    public float speedChangeRate;
    public float playerStartSpeed;
    public float speedChangeAmount;
}