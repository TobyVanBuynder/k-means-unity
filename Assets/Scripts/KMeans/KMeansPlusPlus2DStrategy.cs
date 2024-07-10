using System.Collections.Generic;
using UnityEngine;

public class KMeansPlusPlus2DStrategy : IKMeansStrategy
{
    public KMeans.Stats Execute(List<Transform> dataSet, List<List<Transform>> finalClusters, int numClusters)
    {
        return KMeans.PlusPlus(
            dataSet,
            finalClusters,
            numClusters,
            KMeans.Dimensions.TWO
        );
    }

    public override string ToString()
    {
        return "KMeans PlusPlus";
    }
}