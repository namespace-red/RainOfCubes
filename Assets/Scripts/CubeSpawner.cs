using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Collider))]
public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Transform _cubeParent;
    
    private Collider _spawnZone;
    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _spawnZone = GetComponent<Collider>();
        
        _pool = new ObjectPool<Cube>(OnCreateCube, OnGetCube, OnReleaseCube, OnDestroyCube);
    }

    private void OnEnable()
    {
        StartCoroutine(Run());
    }

    private Cube OnCreateCube()
    {
        Cube newCube = Instantiate(_cubePrefab, _cubeParent);
        newCube.Touched += OnCubeTouched;
        newCube.gameObject.SetActive(false);
        return newCube;
    }
    
    private void OnGetCube(Cube cube)
    {
        cube.transform.position = GetSpawnPosition();
        cube.transform.rotation = Quaternion.identity;
        cube.Init();
        cube.gameObject.SetActive(true);
    }

    private void OnReleaseCube(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.Rigidbody.velocity = Vector3.zero;
    }

    private void OnDestroyCube(Cube cube)
    {
        cube.Touched -= OnCubeTouched;
        Destroy(cube);
    }

    private IEnumerator Run()
    {
        var wait = new WaitForSeconds(_cooldown);
        
        while (enabled)
        {
            _pool.Get();
            yield return wait;
        }
    }

    private void OnCubeTouched(Cube cube)
        => _pool.Release(cube);
    
    private Vector3 GetSpawnPosition()
    {
        var bounds = _spawnZone.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.center.y;

        return new Vector3(x, y, z);
    }
}
