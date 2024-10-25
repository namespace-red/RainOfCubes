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
    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _spawnZone = GetComponent<Collider>();
        
        _pool = new ObjectPool<GameObject>(
            createFunc: Create,
            actionOnGet: Spawn,
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: Destroy
            );
    }

    private void OnEnable()
    {
        StartCoroutine(Run());
    }

    private GameObject Create()
    {
        Cube newCube = Instantiate(_cubePrefab, _cubeParent);
        newCube.Touched += (cube) => { _pool.Release(cube.gameObject); };
        
        GameObject newGameObject = newCube.gameObject;
        newGameObject.SetActive(false);
        return newGameObject;
    }
    
    private void Spawn(GameObject obj)
    {
        obj.transform.position = GetSpawnPosition();
        obj.transform.rotation = Quaternion.identity;
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
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
    
    private Vector3 GetSpawnPosition()
    {
        float x = Random.Range(_spawnZone.bounds.min.x, _spawnZone.bounds.max.x);
        float z = Random.Range(_spawnZone.bounds.min.z, _spawnZone.bounds.max.z);
        float y = _spawnZone.bounds.center.y;

        return new Vector3(x, y, z);
    }
}
