using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class StartMenuScript : MonoBehaviour
{
    [SerializeField] private UIDocument _ui;
    [SerializeField] private GameData _gameData;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private string _sceneName;


    void Start()
    {
        var startButton = _ui.rootVisualElement.Q<Button>("StartButton");
        startButton.clicked += StartGame;

        var difficultyRadioGroup = _ui.rootVisualElement.Q<RadioButtonGroup>("DifficultyRadio");
        difficultyRadioGroup.RegisterValueChangedCallback(ChangeDifficulty);
        SetSpeed(difficultyRadioGroup.value);
    }


    private void StartGame()
    {
        SceneManager.LoadScene(_sceneName);
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