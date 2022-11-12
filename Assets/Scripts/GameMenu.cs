using System;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class GameMenu : Menu
{
    protected override string SceneName => SceneManager.GetActiveScene().name;


    private Label _timeLabel;
    private RadioButtonGroup _difficultyRadioGroup;


    void Awake()
    {
        var startButton = _ui.rootVisualElement.Q<Button>("StartButton");
        startButton.clicked += StartGame;

        _difficultyRadioGroup = _ui.rootVisualElement.Q<RadioButtonGroup>("DifficultyRadio");
        _difficultyRadioGroup.RegisterValueChangedCallback(ChangeDifficulty);

        _timeLabel = _ui.rootVisualElement.Q<Label>("Time");
    }
    private void OnEnable()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(_gameData.time);
        _timeLabel.text = string.Format("{0:D2}:{1:D2}:{2:D3}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        _difficultyRadioGroup.value = _gameData.difficulty;
    }
}