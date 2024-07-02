using UnityEngine;

public class DynamicSpawner : MonoBehaviour
{
    [SerializeField] private Generator _generator;
    [SerializeField] private int _numCurrentSpawn;

    void Awake()
    {
        if (_generator == null)
        {
            enabled = false;
        }
    }

    void Start()
    {
        UpdateGenerator();
    }

    void OnEnable()
    {
        GlobalEvents.UpdateNumCattle += OnUpdateNumCattle;
    }

    void OnDisable()
    {
        GlobalEvents.UpdateNumCattle -= OnUpdateNumCattle;
    }

    private void OnUpdateNumCattle(int count)
    {
        _numCurrentSpawn = count;
        UpdateGenerator();
        GlobalEvents.RunKmeans?.Invoke();
    }

    private void UpdateGenerator()
    {
        _generator.SetNumberOfSpawns(_numCurrentSpawn);
    }
}