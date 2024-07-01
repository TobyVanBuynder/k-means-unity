using System.Collections.Generic;
using UnityEngine;

public class HerdManager : MonoBehaviour
{
    [SerializeField] int _numHerds = 3;
    [SerializeField] IGenerator _generator;

    ICollection<Herd> _herds;
    IHerdFactory _herdFactory;

    void Awake()
    {
        _herds = new List<Herd>();
        _herdFactory = new RandomHerdFactory(0f, 1f, 0.6f, 0.8f, 0.75f, 1f);
        if (_generator == null)
        {
            enabled = false;
        }
    }

    public void SetNumberOfHerds(int numHerds)
    {
        _numHerds = numHerds;

        if (numHerds > _numHerds)
        {
            CreateExtraHerds(numHerds - _numHerds);
        }

        UpdateClusters();
    }

    public void ForceUpdateClusters()
    {
        UpdateClusters();
    }

    void UpdateClusters()
    {
        ClearHerds();
        GenerateClusters();
        UpdateHerds();
    }

    private void ClearHerds()
    {
        foreach (Herd herd in _herds)
        {
            herd.Clear();
        }
    }

    private void GenerateClusters()
    {
        // Kmeans logic
    }

    private void UpdateHerds()
    {
        foreach (Herd herd in _herds)
        {
            herd.Update();
        }
    }

    private void CreateExtraHerds(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _herds.Add(_herdFactory.Create());
        }
    }
}
