using UnityEngine;
using UnityEngine.UIElements;

public class ExitUI : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;

    Button _exitButton;
    Button _fullscreenButton;

    void Awake()
    {
        if (_uiDocument == null)
        {
            enabled = false;
        }
    }

    void Start()
    {
        InitializeVariablesFromRoot(_uiDocument.rootVisualElement);
        HookButtons();
    }

    private void InitializeVariablesFromRoot(VisualElement root)
    {
        _exitButton = root.Q<Button>("ExitButton");
        _fullscreenButton = root.Q<Button>("FullscreenButton");
    }

    private void HookButtons()
    {
        _exitButton.clicked += OnExitButtonClicked;
        _fullscreenButton.clicked += OnFullscreenButtonClicked;
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private void OnFullscreenButtonClicked()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
