using UnityEngine;

public class BombSpawner : SpawnerWithPool<Bomb>
{
    private Vector3 _position;

    public Bomb Spawn(Vector3 position)
    {
        _position = position;
        return Spawn();
    }

    protected override void OnGetObject(Bomb bomb)
    {
        bomb.transform.position = _position;
        base.OnGetObject(bomb);
    }

    protected override void OnReleaseObject(Bomb bomb)
    {
        base.OnReleaseObject(bomb);
        bomb.Rigidbody.velocity = Vector3.zero;
        bomb.transform.rotation = Quaternion.identity;
    }
}