using System;
using TMPro;
using UnityEngine;

public class SpawnerView<T> : MonoBehaviour
    where T : MonoBehaviour, IPoolableObject
{
    [SerializeField] private SpawnerWithPool<T> _spawner;
    [SerializeField] private TMP_Text _createdText;
    [SerializeField] private TMP_Text _spawnedText;
    [SerializeField] private TMP_Text _activeText;

    private void Awake()
    {
        if (_spawner == null)
            throw new NullReferenceException(nameof(_spawner));
        
        if (_createdText == null)
            throw new NullReferenceException(nameof(_createdText));
        
        if (_spawnedText == null)
            throw new NullReferenceException(nameof(_spawnedText));
        
        if (_activeText == null)
            throw new NullReferenceException(nameof(_spawner));
    }

    private void OnEnable()
    {
        _spawner.Created += OnCreated;
        _spawner.Spawned += OnSpawned;
        _spawner.ChangedActive += OnChangedActive;
    }

    private void OnDisable()
    {
        _spawner.Created -= OnCreated;
        _spawner.Spawned -= OnSpawned;
        _spawner.ChangedActive -= OnChangedActive;
    }

    private void OnCreated(int value)
    {
        _createdText.text = value.ToString();
    }

    private void OnSpawned(int value)
    {
        _spawnedText.text = value.ToString();
    }

    private void OnChangedActive(int value)
    {
        _activeText.text = value.ToString();
    }
}
