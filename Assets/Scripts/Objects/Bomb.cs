using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Exploder))]
public class Bomb : MonoBehaviour, IPoolableObject
{
    [SerializeField] private float _minUntilDeath = 2;
    [SerializeField] private float _maxUntilDeath = 5;

    private Material _material;
    private Color _startColor;
    private Exploder _exploder;

    public event Action<IPoolableObject> Released;
    
    public Rigidbody Rigidbody { get; private set; }
    
    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        _startColor = _material.color;
        _exploder = GetComponent<Exploder>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void Init()
    {
        _material.color = _startColor;
    }

    public void BlowUp()
    {
        StartCoroutine(PreparingForExplosion());
    }

    private IEnumerator PreparingForExplosion()
    {
        float untilDeath = Random.Range(_minUntilDeath, _maxUntilDeath + 1);

        while (untilDeath > 0)
        {
            Color color = _material.color;
            color.a = Mathf.MoveTowards(untilDeath, 0, Time.deltaTime);
            _material.color = color;

            yield return null;
            untilDeath -= Time.deltaTime;
        }
        
        _exploder.Run();
        Released?.Invoke(this);
    }
}
