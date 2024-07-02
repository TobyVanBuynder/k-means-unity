using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

using Debug = UnityEngine.Debug;

using TransformList = System.Collections.Generic.List<UnityEngine.Transform>;

public class HerdManager : MonoBehaviour
{
    [SerializeField] int _numActiveHerds = 4;
    [SerializeField] Generator _generator;

    ICollection<Herd> _herds;
    List<TransformList> _cachedClustersList;
    IHerdFactory _herdFactory;

    void Awake()
    {
        if (_generator == null)
        {
            enabled = false;
            return;
        }

        _herds = new List<Herd>(_numActiveHerds);
        _cachedClustersList = new List<TransformList>(_numActiveHerds);
        _herdFactory = new PresetHerdFactory();

        if (_numActiveHerds > 0)
        {
            CreateExtraHerds(_numActiveHerds);
        }
    }

    void OnEnable()
    {
        GlobalEvents.Rescatter += OnRescatter;
        GlobalEvents.RunKmeans += OnRunKmeans;
    }

    void OnDisable()
    {
        GlobalEvents.Rescatter -= OnRescatter;
        GlobalEvents.RunKmeans -= OnRunKmeans;
    }

    private void OnRescatter()
    {
        _generator.ScatterActiveObjects();
        UpdateClusters();
    }

    private void OnRunKmeans()
    {
        UpdateClusters();
    }

    public void SetNumberOfHerds(int numHerds)
    {
        if (numHerds > _numActiveHerds)
        {
            CreateExtraHerds(numHerds - _numActiveHerds);
        }

        _numActiveHerds = numHerds;

        UpdateClusters();
    }

    void UpdateClusters()
    {
        ClearHerds();
        GenerateClusters();
        AssignClustersToHerds();
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
        TransformList activeObjectTransforms = _generator.GetActiveObjects().Select((go) => go.transform).ToList();
        
        KMeans.PlusPlus(
            activeObjectTransforms,
            _cachedClustersList,
            _numActiveHerds,
            KMeans.Dimensions.TWO
        );
    }

    private void AssignClustersToHerds()
    {
        IEnumerator<Herd> herdEnumerator = _herds.GetEnumerator();
        IEnumerator<TransformList> clusterEnumerator = _cachedClustersList.GetEnumerator();

        while (herdEnumerator.MoveNext() && clusterEnumerator.MoveNext())
        {
            AssignClusterToHerd(clusterEnumerator.Current, herdEnumerator.Current);
        }

        herdEnumerator.Dispose();
        clusterEnumerator.Dispose();
    }

    private void AssignClusterToHerd(TransformList cluster, Herd herd)
    {
        foreach (Transform cattleTransform in cluster)
        {
            herd.Assign(cattleTransform.GetComponent<Cattle>());
        }
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
            _cachedClustersList.Add(new TransformList());
        }
    }
}
