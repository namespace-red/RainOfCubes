using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour, IPoolableObject
{
    [SerializeField] private float _minUntilDeath = 2f;
    [SerializeField] private float _maxUntilDeath = 5f;
    
    private Material _material;
    private Color _startColor;
    private bool _isContacted;

    public event Action<IPoolableObject> Released; 
    
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        Rigidbody = GetComponent<Rigidbody>();
        _startColor = _material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isContacted)
            return;
        
        if (collision.gameObject.GetComponent<Platform>() == false)
            return;

        _isContacted = true;
        _material.color = GetColor();
        
        StartCoroutine(ThrowReleasedEvent());
    }

    public void Init()
    {
        _isContacted = false;
        _material.color = _startColor;
    }

    private Color GetColor()
        => Random.ColorHSV();
    
    private IEnumerator ThrowReleasedEvent()
    {
        float untilDeath = Random.Range(_minUntilDeath, _maxUntilDeath);
        yield return new WaitForSeconds(untilDeath);
        Released?.Invoke(this);
    }
}
