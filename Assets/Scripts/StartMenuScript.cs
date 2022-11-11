using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class StartMenuScript : MonoBehaviour
{
    [SerializeField] private UIDocument _ui;
    [SerializeField] private GameData _gameData;
    [SerializeField] private string _sceneName;


    void Start()
    {
        var startButton = _ui.rootVisualElement.Q<Button>("StartButton");
        startButton.clicked += StartGame;

        var difficultyRadioGroup = _ui.rootVisualElement.Q<RadioButtonGroup>("DifficultyRadio");
        difficultyRadioGroup.RegisterValueChangedCallback(ChangeDifficulty);
        _gameData.difficulty = difficultyRadioGroup.value;
    }


    private void StartGame()
    {
        SceneManager.LoadScene(_sceneName);
    }
    private void ChangeDifficulty(ChangeEvent<int> evt)
    {
        _gameData.difficulty = evt.newValue;
    }
}