using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IGenerator
{
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private float _spawnRadius = 20f;

    // TODO: use object pooling instead
    private ICollection<GameObject> _spawnedObjects;
    private int _numActiveObjects = 0;


    void Awake()
    {
        if (_prefabToSpawn == null || _parentTransform == null) {
            enabled = false;
        }

        _spawnedObjects = new List<GameObject>();
    }

    public void Spawn(int count)
    {
        if (count < 1)
        {
            return;
        }
        
        if (count > _spawnedObjects.Count)
        {
            CreateNewObjects(count - _spawnedObjects.Count);
        }
        
        UpdateActiveObjects(count);
    }

    public List<GameObject> GetActiveObjects()
    {
        List<GameObject> list = new List<GameObject>(GetActiveCount());

        IEnumerator<GameObject> iterator = _spawnedObjects.GetEnumerator();
        for (int i = 0; i < GetActiveCount(); i++)
        {
            iterator.MoveNext();
            list.Add(iterator.Current);
        }
        iterator.Dispose();

        return list;
    }

    public int GetActiveCount()
    {
        return _numActiveObjects;
    }

    public void ScatterActiveObjects()
    {
        IEnumerator<GameObject> iterator = _spawnedObjects.GetEnumerator();

        for (int i = 0; i < GetActiveCount(); i++)
        {
            iterator.MoveNext();
            iterator.Current.transform.SetPositionAndRotation(
                Utils.RandomPositionInCircle(GetFloorPosition(), _spawnRadius),
                Utils.RandomScaledQuaternion(Vector3.up)
            );
        }

        iterator.Dispose();
    }

    private void CreateNewObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _spawnedObjects.Add(InstantiateSpawnObject());
        }
    }

    private void UpdateActiveObjects(int count)
    {
        int activeObjectCount = count;
        int inactiveObjectCount = _spawnedObjects.Count - count;

        IEnumerator<GameObject> iterator = _spawnedObjects.GetEnumerator();
        SetNumObjectsActiveState(iterator, activeObjectCount, true);
        SetNumObjectsActiveState(iterator, inactiveObjectCount, false);
        iterator.Dispose();

        _numActiveObjects = activeObjectCount;
    }

    private void SetNumObjectsActiveState(IEnumerator<GameObject> iterator, int count, bool activeState)
    {
        for (int i = 0; i < count; i++)
        {
            iterator.MoveNext();
            iterator.Current.SetActive(activeState);
        }
    }

    private Vector3 GetFloorPosition()
    {
        Vector3 floorPosition = transform.position;
        floorPosition.y = 0;
        return floorPosition;
    }

    private GameObject InstantiateSpawnObject()
    {
        return Instantiate(
            _prefabToSpawn,
            Utils.RandomPositionInCircle(GetFloorPosition(), _spawnRadius),
            Utils.RandomScaledQuaternion(Vector3.up),
            _parentTransform
        );
    }


    #if UNITY_EDITOR
    [Header("EDITOR")]
    [SerializeField] private Color _gizmoColor = Color.white;

    void OnDrawGizmos() {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }
    #endif
}
