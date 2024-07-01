using System.Collections.Generic;
using UnityEngine;

public interface IGenerator
{
    List<GameObject> GetActiveObjects();
    void Spawn(int count);
    int GetActiveCount();
}