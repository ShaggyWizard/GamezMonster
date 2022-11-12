
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class GameMenu : MonoBehaviour
{
    [SerializeField] private UIDocument _ui;
    [SerializeField] private GameData _gameData;
    [SerializeField] private GameSettings _gameSettings;


    private Label _timeLabel;
    void Awake()
    {
        var startButton = _ui.rootVisualElement.Q<Button>("StartButton");
        startButton.clicked += StartGame;

        var difficultyRadioGroup = _ui.rootVisualElement.Q<RadioButtonGroup>("DifficultyRadio");
        difficultyRadioGroup.RegisterValueChangedCallback(ChangeDifficulty);
        difficultyRadioGroup.SetValueWithoutNotify(_gameSettings.difficulty);

        _timeLabel = _ui.rootVisualElement.Q<Label>("Time");
    }
    private void OnEnable()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(_gameData.time);
        _timeLabel.text = string.Format("{0:D2}:{1:D2}:{2:D3}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
    }


    private void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void ChangeDifficulty(ChangeEvent<int> evt)
    {
        SetSpeed(evt.newValue);
    }
    private void SetSpeed(int difficulty)
    {
        _gameData.Speed = _gameSettings.difficulties[difficulty];
    }
}