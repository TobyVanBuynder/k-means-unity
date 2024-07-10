using System.Diagnostics;
using UnityEngine;

public class KMeansTimer : MonoBehaviour
{
    Stopwatch _stopwatch;
    
    void OnEnable()
    {
        GlobalEvents.BeforeKmeans += OnBeforeKmeans;
        GlobalEvents.AfterKmeans += OnAfterKmeans;
    }

    void OnDisable()
    {
        GlobalEvents.BeforeKmeans -= OnBeforeKmeans;
        GlobalEvents.AfterKmeans -= OnAfterKmeans;
    }

    void OnBeforeKmeans()
    {
        _stopwatch = Stopwatch.StartNew();
    }

    void OnAfterKmeans(KMeans.Stats kmeansStats, string kmeansName)
    {
        _stopwatch.Stop();
        float milliseconds = Utils.Math.ConvertTicksToMilliseconds(_stopwatch.ElapsedTicks);
        GlobalEvents.KmeansTimeTaken?.Invoke(kmeansStats, kmeansName, milliseconds);
    }
}