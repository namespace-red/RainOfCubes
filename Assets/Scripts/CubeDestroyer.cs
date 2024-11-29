using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CubeDestroer : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Cube cube))
            Destroy(cube.gameObject);
    }
}
