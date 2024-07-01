using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private float _spawnRadius = 1f;

    // TODO: use object pooling instead
    List<GameObject> _spawnedObjects;

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
        
        SetObjectsToActive(count);
    }

    private void CreateNewObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _spawnedObjects.Add(InstantiateSpawnObject());
        }
    }

    private void SetObjectsToActive(int count)
    {
        for (int a = 0; a < count; a++)
        {
            _spawnedObjects[a].SetActive(true);
        }
        for (int b = count; b < _spawnedObjects.Count; b++)
        {
            _spawnedObjects[b].SetActive(false);
        }
    }

    private GameObject InstantiateSpawnObject()
    {
        Vector3 floorPosition = transform.position;
        floorPosition.y = 0;

        return Instantiate(
            _prefabToSpawn,
            Utils.RandomPositionInCircle(floorPosition, _spawnRadius),
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

    // Object pooling code
    /* private ObjectPool<GameObject> _objectPool;
    _objectPool = new ObjectPool<GameObject>(OnCreatePooledObject, OnGetPooledObject, OnReleasePooledObject, OnDestroyPooledObject);
    private GameObject OnCreatePooledObject()
    {
        return Instantiate(_prefabToSpawn, GetRandomPosition(), Quaternion.identity, _parentTransform);
    }
    private void OnGetPooledObject(GameObject @object)
    {
        @object.SetActive(true);
    }
    private void OnReleasePooledObject(GameObject @object)
    {
        @object.SetActive(false);
    }
    private void OnDestroyPooledObject(GameObject @object)
    {
        Destroy(@object);
    } */
}
