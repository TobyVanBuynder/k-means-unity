using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class RNGUnique : IDisposable
{
    HashSet<int> _hashSet = new HashSet<int>();

    public RNGUnique(int capacity)
    {
        // Makes sure that the HashSet doesn't grow while generating
        _hashSet.EnsureCapacity(capacity);
    }

    public HashSet<int> Generate(int count, int minValue, int maxValue)
    {
        // Ensure an empty set
        _hashSet.Clear();

        // Keep generating unique random numbers until count
        while (_hashSet.Count != count)
        {
            _hashSet.Add(Random.Range(minValue, maxValue));
        }

        return _hashSet;
    }

    public void Dispose()
    {
        _hashSet.Clear();
    }
}
