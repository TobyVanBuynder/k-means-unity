using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Cluster = System.Collections.Generic.List<UnityEngine.Transform>;

public class HerdManager : MonoBehaviour
{
    [SerializeField] int _numActiveHerds = 4;
    [SerializeField] Generator _generator;

    ICollection<Herd> _herds;
    List<Cluster> _cachedClustersList;
    IHerdFactory _herdFactory;

    void Awake()
    {
        if (_generator == null)
        {
            enabled = false;
            return;
        }

        _herds = new List<Herd>(_numActiveHerds);
        _cachedClustersList = new List<Cluster>(_numActiveHerds);
        _herdFactory = new RandomHerdFactory(0f, 1f, 0.6f, 1f, 0f, 1f);

        if (_numActiveHerds > 0)
        {
            CreateExtraHerds(_numActiveHerds);
        }
    }

    // TODO: implement with UI
    void SetNumberOfHerds(int numHerds)
    {
        if (numHerds > _numActiveHerds)
        {
            CreateExtraHerds(numHerds - _numActiveHerds);
        }

        _numActiveHerds = numHerds;

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
        List<Transform> activeObjectTransforms = _generator.GetActiveObjects().Select((go) => go.transform).ToList();
        
        KMeans.Naive(
            activeObjectTransforms,
            _cachedClustersList,
            _numActiveHerds,
            KMeans.Dimensions.TWO
        );
    }

    private void AssignClustersToHerds()
    {
        IEnumerator<Herd> herdEnumerator = _herds.GetEnumerator();
        IEnumerator<Cluster> clusterEnumerator = _cachedClustersList.GetEnumerator();

        while (herdEnumerator.MoveNext() && clusterEnumerator.MoveNext())
        {
            AssignClusterToHerd(clusterEnumerator.Current, herdEnumerator.Current);
        }
    }

    private void AssignClusterToHerd(Cluster cluster, Herd herd)
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
            _cachedClustersList.Add(new Cluster());
        }
    }
}
