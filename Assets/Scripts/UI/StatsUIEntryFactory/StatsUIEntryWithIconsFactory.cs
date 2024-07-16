using UnityEngine;
using UnityEngine.UIElements;

public class StatsUIEntryWithIconsFactory : IKMeansStatsUIEntryFactory
{
    VisualTreeAsset _statsEntryPrototype;

    public StatsUIEntryWithIconsFactory(VisualTreeAsset statsEntryPrototype)
    {
        _statsEntryPrototype = statsEntryPrototype;
    }

    public VisualElement Create(KMeans.Stats kmeansStats, string kmeansName, float milliseconds)
    {
        if (_statsEntryPrototype == null) return new VisualElement();

        VisualElement statsRoot = _statsEntryPrototype.Instantiate();

        SetupStatsRoot(statsRoot, kmeansStats, kmeansName, milliseconds);

        return statsRoot;
    }

    private void SetupStatsRoot(VisualElement statsRoot, KMeans.Stats kmeansStats, string kmeansName, float milliseconds)
    {
        Label typeLabel = statsRoot.Q<Label>("TypeLabel");
        typeLabel.text = kmeansName;

        VisualElement timeStatElement = GetVisualElement(statsRoot, "TimeStat");
        SetStatLabel(timeStatElement, $"{milliseconds} ms");

        VisualElement numIterationsStatElement = GetVisualElement(statsRoot, "NumIterationsStat");
        SetStatLabel(numIterationsStatElement, kmeansStats.NumIterations.ToString());

        VisualElement errorRateStatElement = GetVisualElement(statsRoot, "ErrorRateStat");
        SetStatLabel(errorRateStatElement, $"{kmeansStats.ErrorRate} error");

        VisualElement distributionsStatElement = GetVisualElement(statsRoot, "DistributionsStat");
        SetStatLabel(distributionsStatElement, GetDistributionsString(kmeansStats.Distributions));
    }

    private VisualElement GetVisualElement(VisualElement root, string elementName)
    {
        return root.Q<VisualElement>(elementName);
    }

    private void SetStatLabel(VisualElement statElement, string text)
    {
        Label statText = statElement.Q<Label>("Text");
        statText.text = text;
    }

    private string GetDistributionsString(float[] distributions)
    {
        float[] normalizedDistributions = distributions;
        NormalizeDistributions(ref normalizedDistributions);
        return $"[{string.Join(',', normalizedDistributions)}]";
    }

    private void NormalizeDistributions(ref float[] distributions)
    {
        for (int i = 0; i < distributions.Length; i++)
        {
            distributions[i] = Mathf.Round(distributions[i]);
        }
    }
}