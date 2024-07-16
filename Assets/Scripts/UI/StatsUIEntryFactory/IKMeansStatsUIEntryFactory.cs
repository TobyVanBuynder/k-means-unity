using UnityEngine.UIElements;

public interface IKMeansStatsUIEntryFactory
{
    VisualElement Create(KMeans.Stats kmeansStats, string kmeansName, float milliseconds);
}