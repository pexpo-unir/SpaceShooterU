using UnityEngine;

public interface IPoolableObject<T> where T : Component
{
    public void ReturnToPool();
}
