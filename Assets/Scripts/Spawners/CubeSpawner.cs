using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IPosition))]
public class CubeSpawner : SpawnerWithPool<Cube>
{
    [SerializeField] private float _cooldown;
    
    private IPosition _spawnPosition;

    protected override void Awake()
    {
        base.Awake();

        _spawnPosition = GetComponent<IPosition>();
    }

    private void OnEnable()
    {
        StartCoroutine(Run());
    }

    protected override Cube OnCreateObject()
    {
        Cube newCube = base.OnCreateObject();
        newCube.Finished += OnCubeFinished;
        return newCube;
    }

    protected override void OnGetObject(Cube cube)
    {
        cube.transform.position = _spawnPosition.Get();
        cube.Init();
        cube.gameObject.SetActive(true);
    }

    protected override void OnReleaseObject(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.Rigidbody.velocity = Vector3.zero;
        cube.transform.rotation = Quaternion.identity;
    }

    protected override void OnDestroyObject(Cube cube)
    {
        cube.Finished -= OnCubeFinished;
        Destroy(cube);
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

    private void OnCubeFinished(Cube cube)
    {
        Release(cube);
    }
}
