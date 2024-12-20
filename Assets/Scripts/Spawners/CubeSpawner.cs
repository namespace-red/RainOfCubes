using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IPosition))]
public class CubeSpawner : SpawnerWithPool<Cube>
{
    [SerializeField] private float _cooldown;
    [SerializeField] private BombSpawner _bombSpawner;
    
    private IPosition _spawnPosition;

    protected override void Awake()
    {
        base.Awake();

        _spawnPosition = GetComponent<IPosition>();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        
        if (_bombSpawner == null)
            throw new NullReferenceException(nameof(_bombSpawner));
    }

    private void OnEnable()
    {
        StartCoroutine(Run());
    }

    protected override void OnGetObject(Cube cube)
    {
        cube.transform.position = _spawnPosition.Get();
        base.OnGetObject(cube);
    }

    protected override void OnReleaseObject(Cube cube)
    {
        base.OnReleaseObject(cube);
        
        var  bomb = _bombSpawner.Spawn(cube.transform.position);
        bomb.Rigidbody.velocity = cube.Rigidbody.velocity;
        bomb.BlowUp();
        
        cube.Rigidbody.velocity = Vector3.zero;
        cube.transform.rotation = Quaternion.identity;
    }

    private IEnumerator Run()
    {
        var wait = new WaitForSeconds(_cooldown);
        
        while (enabled)
        {
            Spawn();
            yield return wait;
        }
    }
}
