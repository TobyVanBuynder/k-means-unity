using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIMenuPanel : MonoBehaviour
{
    [SerializeField] UIDocument _uiMenuPanel;

    SliderInt _herdSlider;
    SliderInt _cattleSlider;

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
        GetElementsFromPanel(_uiMenuPanel.rootVisualElement);
        HookIntoElements();
    }

    private void GetElementsFromPanel(VisualElement root)
    {
        _herdSlider = root.Q<SliderInt>("NumHerdSlider");
        _cattleSlider = root.Q<SliderInt>("NumCattleSlider");

        _rescatterButton = root.Q<Button>("RescatterButton");
        _runKmeansButton = root.Q<Button>("RunKmeansButton");
    }

    private void HookIntoElements()
    {
        _herdSlider.RegisterValueChangedCallback(OnHerdSliderChanged);
        _cattleSlider.RegisterValueChangedCallback(OnCattleSliderChanged);

        _rescatterButton.RegisterCallback<ClickEvent>(OnRescatterButtonClicked);
        _runKmeansButton.RegisterCallback<ClickEvent>(OnRunKmeansButtonClicked);
    }

    private void OnRescatterButtonClicked(ClickEvent _)
    {
        GlobalEvents.Rescatter?.Invoke();
    }

    private void OnRunKmeansButtonClicked(ClickEvent _)
    {
        GlobalEvents.RunKmeans?.Invoke();
    }

    private void OnHerdSliderChanged(ChangeEvent<int> evt)
    {
        GlobalEvents.UpdateNumHerds?.Invoke(evt.newValue);
    }

    private void OnCattleSliderChanged(ChangeEvent<int> evt)
    {
        GlobalEvents.UpdateNumCattle?.Invoke(evt.newValue);
    }
}
