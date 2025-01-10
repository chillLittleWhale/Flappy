using UnityEngine;

namespace AjaxNguyen.Core.ObjectPooling
{
    public class ReturnToPool : MonoBehaviour
    {
        public ObjectPool pool;

        public void OnDisable()
        {
            pool.AddToPool(gameObject);
        }
    }
}
