using System.Collections.Generic;
using UnityEngine;

public interface IKMeansStrategy
{
    KMeans.Stats Execute(List<Transform> dataSet, List<List<Transform>> finalClusters, int numClusters);
}