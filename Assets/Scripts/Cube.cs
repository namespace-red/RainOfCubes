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
    private bool _isContacted;

    public event Action<Cube> Touched; 

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isContacted)
            return;
        
        if (collision.gameObject.GetComponent<Platform>() == false)
            return;

        _isContacted = true;
        _material.color = GetColor();

        float untilDeath = Random.Range(_minUntilDeath, _maxUntilDeath);
        Invoke(nameof(ThrowTouchedEvent) , untilDeath);
    }
    
    private Color GetColor()
        => Random.ColorHSV();

    private void ThrowTouchedEvent()
    {
        Touched?.Invoke(this);
    }
}
