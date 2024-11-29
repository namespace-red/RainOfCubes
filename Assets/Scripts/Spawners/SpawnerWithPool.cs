using System;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnerWithPool<T> : MonoBehaviour
    where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private Transform _prefabParent;

    private ObjectPool<T> _pool;

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
        return _pool.Get();
    }

    public virtual void Release(T obj)
    {
        _pool.Release(obj);
    }

    protected virtual T OnCreateObject()
    {
        var obj = Instantiate(_prefab, _prefabParent);
        obj.gameObject.SetActive(false);
        return obj;
    }

    protected virtual void OnGetObject(T obj)
    {
        obj.gameObject.SetActive(true);
    }

    protected virtual void OnReleaseObject(T obj)
    {
        obj.gameObject.SetActive(false);
    }
    
    protected virtual void OnDestroyObject(T obj)
    {
        Destroy(obj);
    }
}
