using System.Collections.Generic;
using UnityEngine;

public interface IGenerator
{
    List<GameObject> GetActiveObjects();
    void SetNumberOfSpawns(int count);
    int GetActiveCount();
}