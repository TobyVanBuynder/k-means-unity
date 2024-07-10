using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanelUI : MonoBehaviour
{
    [SerializeField] UIDocument _uiDocument;

    SliderInt _herdSlider;
    Label _numHerdsLabel;

    SliderInt _cattleSlider;
    Label _numCattleLabel;
    
    EnumField _kmeansTypeEnum;

    Button _rescatterButton;
    Button _runKmeansButton;

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

        _kmeansTypeEnum = root.Q<EnumField>("KMeansTypeEnum");

        _rescatterButton = root.Q<Button>("RescatterButton");
        _runKmeansButton = root.Q<Button>("RunKmeansButton");
    }

    private void HookIntoEvents()
    {
        _herdSlider.RegisterValueChangedCallback(OnHerdSliderChanged);
        _cattleSlider.RegisterValueChangedCallback(OnCattleSliderChanged);

        _kmeansTypeEnum.RegisterValueChangedCallback(OnKmeansTypeChanged);

        _rescatterButton.clicked += OnRescatterButtonClicked;
        _runKmeansButton.clicked += OnRunKmeansButtonClicked;
    }

    private void OnKmeansTypeChanged(ChangeEvent<Enum> evt)
    {
        GlobalEvents.ChangeKmeansType?.Invoke((KMeansType)evt.newValue);
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
