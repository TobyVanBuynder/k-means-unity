using System.Collections.Generic;
using UnityEngine;

public class KMeansNullStrategy : IKMeansStrategy
{
    public KMeans.Stats Execute(List<Transform> _, List<List<Transform>> __, int ___)
    {
        return KMeans.Stats.Null;
    }

    public override string ToString()
    {
        return "Null";
    }
}