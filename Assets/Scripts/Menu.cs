using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public abstract class Menu : MonoBehaviour
{
    [SerializeField] protected UIDocument _ui;
    [SerializeField] protected GameData _gameData;
    [SerializeField] protected GameSettings _gameSettings;


    protected abstract string SceneName { get; }


    protected void ChangeDifficulty(ChangeEvent<int> evt)
    {
        ChangeDifficulty(evt.newValue);
    }
    protected void ChangeDifficulty(int difficulty)
    {
        int clampDifficulty = Mathf.Clamp(difficulty, 0, _gameSettings.difficulties.Count - 1);
        _gameData.difficulty = clampDifficulty;
        _gameData.Speed = _gameSettings.difficulties[clampDifficulty];
    }
    protected void StartGame()
    {
        SceneManager.LoadScene(SceneName);
    }
}