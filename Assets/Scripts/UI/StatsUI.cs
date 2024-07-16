using UnityEngine;
using UnityEngine.UIElements;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private VisualTreeAsset _statsEntryPrototype;

    private IKMeansStatsUIEntryFactory _statsEntryFactory;
    private ScrollView _statsList;

    void Awake()
    {
        InitializeVariablesFromRoot(_uiDocument.rootVisualElement);

        _statsEntryFactory = new StatsUIEntryWithIconsFactory(_statsEntryPrototype);
    }

    private void InitializeVariablesFromRoot(VisualElement root)
    {
        _statsList = root.Q<ScrollView>("StatsList");
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
        VisualElement stats = _statsEntryFactory.Create(kmeansStats, kmeansName, milliseconds);
        _statsList.contentContainer.Add(stats);
    }
}