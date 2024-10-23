using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Transform _cubeParent;
    private Collider _spawnZone;

    private void Awake()
    {
        _spawnZone = GetComponent<Collider>();

        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        var wait = new WaitForSeconds(_cooldown);
        
        while (enabled)
        {
            Instantiate(_cubePrefab, GetSpawnPosition(), Quaternion.identity, _cubeParent);
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
