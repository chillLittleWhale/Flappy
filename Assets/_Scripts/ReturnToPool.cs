using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    public ObjectPool pool;

    public void OnDisable()
    {
        pool.AddToPool(gameObject);
    }
}
