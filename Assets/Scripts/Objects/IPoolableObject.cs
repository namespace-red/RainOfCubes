using System;

public interface IPoolableObject
{
    event Action<IPoolableObject> Released;

    void Init();
}
