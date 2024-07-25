using System.Collections.Generic;
using UnityEngine;

public class KMeansNaive2DStrategy : IKMeansStrategy
{
    public KMeans.Stats Execute(List<Transform> dataSet, List<List<Transform>> finalClusters, int numClusters)
    {
        return KMeans.Naive(
            dataSet,
            finalClusters,
            numClusters
        );
    }

    public override string ToString()
    {
        return "KMeans Naïve";
    }
}