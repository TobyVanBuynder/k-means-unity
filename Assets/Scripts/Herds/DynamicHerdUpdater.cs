using UnityEngine;

public class DynamicHerdUpdater : MonoBehaviour
{
    [SerializeField] HerdManager _herdManager;

    void Awake()
    {
        if (_herdManager == null)
        {
            enabled = false;
        }
    }

    void OnEnable()
    {
        GlobalEvents.UpdateNumHerds += OnUpdateNumHerds;
    }

    void OnDisable()
    {
        GlobalEvents.UpdateNumHerds -= OnUpdateNumHerds;
    }

    private void OnUpdateNumHerds(int count)
    {
        _herdManager.SetNumberOfHerds(count);
    }
}