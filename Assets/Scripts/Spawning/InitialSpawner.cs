using UnityEngine;

public class InitialSpawner : MonoBehaviour
{
    [SerializeField] private Generator _generator;
    [SerializeField] private int _numInitialSpawns = 0;

    void Awake()
    {
        if (_generator == null)
        {
            enabled = false;
        }
    }

    void Start()
    {
        if (_numInitialSpawns > 0)
        {
            _generator.SetNumberOfSpawns(_numInitialSpawns);
        }
    }
}
