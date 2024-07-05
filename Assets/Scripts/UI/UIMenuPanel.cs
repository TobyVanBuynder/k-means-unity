using UnityEngine;
using UnityEngine.UIElements;

public class UIMenuPanel : MonoBehaviour
{
    [SerializeField] UIDocument _uiMenuPanel;

    SliderInt _herdSlider;
    Label _numHerdsLabel;

    SliderInt _cattleSlider;
    Label _numCattleLabel;

    Button _rescatterButton;
    Button _runKmeansButton;

    void Awake()
    {
        if (_uiMenuPanel == null)
        {
            enabled = false;
        }
    }

    void Start()
    {
        InitializeVariablesFromRoot(_uiMenuPanel.rootVisualElement);
        HookIntoEvents();
        UpdateNumHerdsLabel(_herdSlider.value);
        UpdateNumCattleLabel(_cattleSlider.value);
    }

    private void InitializeVariablesFromRoot(VisualElement root)
    {
        _herdSlider = root.Q<SliderInt>("NumHerdSlider");
        _numHerdsLabel = root.Q<Label>("NumHerds");

        _cattleSlider = root.Q<SliderInt>("NumCattleSlider");
        _numCattleLabel = root.Q<Label>("NumCattle");

        _rescatterButton = root.Q<Button>("RescatterButton");
        _runKmeansButton = root.Q<Button>("RunKmeansButton");
    }

    private void HookIntoEvents()
    {
        _herdSlider.RegisterValueChangedCallback(OnHerdSliderChanged);

        _cattleSlider.RegisterValueChangedCallback(OnCattleSliderChanged);

        _rescatterButton.clicked += OnRescatterButtonClicked;
        _runKmeansButton.clicked += OnRunKmeansButtonClicked;
    }

    private void OnRescatterButtonClicked()
    {
        GlobalEvents.Rescatter?.Invoke();
    }

    private void OnRunKmeansButtonClicked()
    {
        GlobalEvents.RunKmeans?.Invoke();
    }

    private void OnHerdSliderChanged(ChangeEvent<int> evt)
    {
        UpdateNumHerdsLabel(evt.newValue);
        GlobalEvents.UpdateNumHerds?.Invoke(evt.newValue);
    }

    private void OnCattleSliderChanged(ChangeEvent<int> evt)
    {
        UpdateNumCattleLabel(evt.newValue);
        GlobalEvents.UpdateNumCattle?.Invoke(evt.newValue);
    }

    private void UpdateNumHerdsLabel(int newValue)
    {
        _numHerdsLabel.text = newValue.ToString();
    }

    private void UpdateNumCattleLabel(int newValue)
    {
        _numCattleLabel.text = newValue.ToString();
    }
}
