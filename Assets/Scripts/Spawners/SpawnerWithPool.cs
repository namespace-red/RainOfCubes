using System;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnerWithPool<T> : MonoBehaviour
    where T : MonoBehaviour, IPoolableObject
{
    [SerializeField] private T _prefab;
    [SerializeField] private Transform _prefabParent;

    private ObjectPool<T> _pool;
    private int _spawnedCount;
    
    public event Action<int> Created;
    public event Action<int> Spawned;
    public event Action<int> ChangedActive;

    protected virtual void Awake()
    {
        if (_prefab == null)
            throw new NullReferenceException(nameof(_prefab));
        
        if (_prefabParent == null)
            throw new NullReferenceException(nameof(_prefabParent));

        _pool = new ObjectPool<T>(OnCreateObject, OnGetObject, OnReleaseObject, OnDestroyObject);
    }

    public virtual T Spawn()
    {
        Spawned?.Invoke(++_spawnedCount);
        ChangedActive?.Invoke(_pool.CountActive);
        return _pool.Get();
    }

    public virtual void Release(T obj)
    {
        _pool.Release(obj);
        ChangedActive?.Invoke(_pool.CountActive);
    }

    protected virtual T OnCreateObject()
    {
        var obj = Instantiate(_prefab, _prefabParent);
        obj.gameObject.SetActive(false);

        obj.Released += OnObjectReleased;
        Created?.Invoke(_pool.CountAll);
        return obj;
    }

    protected virtual void OnGetObject(T obj)
    {
        obj.Init();
        obj.gameObject.SetActive(true);
    }

    protected virtual void OnReleaseObject(T obj)
    {
        obj.gameObject.SetActive(false);
    }
    
    protected virtual void OnDestroyObject(T obj)
    {
        obj.Released -= OnObjectReleased;
        Destroy(obj);
    }

    private void OnObjectReleased(IPoolableObject obj)
    {
        Release((T)obj);
    }
}
