using System;
using UnityEngine;
using UnityEngine.UIElements;

public class StatsUI : MonoBehaviour
{
    [SerializeField] UIDocument _uiDocument;

    ListView _statsList;

    void Awake()
    {
        InitializeVariablesFromRoot(_uiDocument.rootVisualElement);
    }

    private void InitializeVariablesFromRoot(VisualElement root)
    {
        _statsList = root.Q<ListView>("StatsList");
    }

    void OnEnable()
    {
        GlobalEvents.KmeansTimeTaken += OnKmeansTimeTaken;
    }

    void OnDisable()
    {
        GlobalEvents.KmeansTimeTaken -= OnKmeansTimeTaken;
    }

    void OnKmeansTimeTaken(KMeans.Stats kmeansStats, string kmeansName, float milliseconds)
    {
        Label newLabel = new Label(kmeansName + " took <b>" + milliseconds + "ms</b> :\n" + kmeansStats.ToString());
        newLabel.AddToClassList("stats-list__entry");
        _statsList.hierarchy.Add(newLabel);
    }
}