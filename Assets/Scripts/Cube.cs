using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private float _minUntilDeath = 2f;
    [SerializeField] private float _maxUntilDeath = 5f;
    
    private Material _material;
    private Color _startColor;
    private bool _isContacted;

    public event Action<Cube> Touched; 
    
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
        
        StartCoroutine(ThrowTouchedEvent());
    }

    public void Init()
    {
        _isContacted = false;
        _material.color = _startColor;
    }

    private Color GetColor()
        => Random.ColorHSV();
    
    private IEnumerator ThrowTouchedEvent()
    {
        float untilDeath = Random.Range(_minUntilDeath, _maxUntilDeath + 1);
        yield return new WaitForSeconds(untilDeath);
        Touched?.Invoke(this);
    }
}
