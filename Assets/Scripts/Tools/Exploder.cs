using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private float _radius;
    
    public void Run()
    {
        foreach (var repelledObject in TakeRepelled())
        {
            repelledObject.AddExplosionForce(_force, transform.position, _radius);
        }
    }

    private IEnumerable<Rigidbody> TakeRepelled()
    {
        var repelledObjects = new List<Rigidbody>();
        
        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, _radius);

        foreach (var collider in overlappedColliders)
        {
            if (collider.attachedRigidbody != null)
            {
                repelledObjects.Add(collider.attachedRigidbody);
            }
        }

        return repelledObjects;
    }
}
