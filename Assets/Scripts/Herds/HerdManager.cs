using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using TransformList = System.Collections.Generic.List<UnityEngine.Transform>;

public class HerdManager : MonoBehaviour
{
    [SerializeField] int _numActiveHerds = 4;
    [SerializeField] Generator _generator;

    ICollection<Herd> _herds;
    List<TransformList> _cachedClustersList;
    IHerdFactory _herdFactory;
    IKMeansStrategy _kMeansStrategy;
    IKMeansStrategyFactory _kMeansStrategyFactory;

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
        _kMeansStrategyFactory = new KMeans2DStrategyFactory();
        _kMeansStrategy = _kMeansStrategyFactory.Create(KMeansType.Naive);

        if (_numActiveHerds > 0)
        {
            CreateExtraHerds(_numActiveHerds);
        }
    }

    void OnEnable()
    {
        GlobalEvents.ChangeKmeansType += OnChangeKmeansType;
        GlobalEvents.Rescatter += OnRescatter;
        GlobalEvents.RunKmeans += OnRunKmeans;
    }

    void OnDisable()
    {
        GlobalEvents.ChangeKmeansType -= OnChangeKmeansType;
        GlobalEvents.Rescatter -= OnRescatter;
        GlobalEvents.RunKmeans -= OnRunKmeans;
    }

    private void OnChangeKmeansType(KMeansType type)
    {
        _kMeansStrategy = _kMeansStrategyFactory.Create(type);
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

        GlobalEvents.BeforeKmeans?.Invoke();
        KMeans.Stats kmeansStats = _kMeansStrategy.Execute(activeObjectTransforms, _cachedClustersList, _numActiveHerds);
        GlobalEvents.AfterKmeans?.Invoke(kmeansStats, _kMeansStrategy.ToString());
    }

    private void AssignClustersToHerds()
    {
        IEnumerator<Herd> herdEnumerator = _herds.GetEnumerator();
        IEnumerator<TransformList> clusterEnumerator = _cachedClustersList.GetEnumerator();

        int currentClusterIndex = 0;
        while (herdEnumerator.MoveNext() && clusterEnumerator.MoveNext() && currentClusterIndex < _numActiveHerds)
        {
            AssignClusterToHerd(clusterEnumerator.Current, herdEnumerator.Current);
            currentClusterIndex++;
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
