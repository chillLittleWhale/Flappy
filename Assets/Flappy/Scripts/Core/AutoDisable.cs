using UnityEngine;

namespace Flappy.Core
{
    public class AutoDisable : MonoBehaviour
    {
        [SerializeField] float X_EndPoint;

        void Update()
        {
            if (transform.position.x < X_EndPoint)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
