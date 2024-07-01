using UnityEngine;

public class InitialSpawner : MonoBehaviour
{
    [SerializeField] private Generator _generator;
    [SerializeField] private int _numInitialSpawn;

    void Awake()
    {
        if (_generator == null || _numInitialSpawn == 0)
        {
            enabled = false;
        }
    }

    void Start()
    {
        _generator.Spawn(_numInitialSpawn);
    }
}
