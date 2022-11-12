using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class StartMenu : Menu
{
    [SerializeField] private string _sceneName;


    protected override string SceneName => _sceneName;


    void Start()
    {
        var startButton = _ui.rootVisualElement.Q<Button>("StartButton");
        startButton.clicked += StartGame;

        var difficultyRadioGroup = _ui.rootVisualElement.Q<RadioButtonGroup>("DifficultyRadio");
        difficultyRadioGroup.RegisterValueChangedCallback(ChangeDifficulty);
        ChangeDifficulty(difficultyRadioGroup.value);
    }
}