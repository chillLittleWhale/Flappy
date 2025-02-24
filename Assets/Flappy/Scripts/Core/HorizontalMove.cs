using UnityEngine;

namespace Flappy.Core
{
    public class HorizontalMove : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] bool active = false;

        void Update()
        {
            if (!active) return;

            transform.position += speed * Time.deltaTime * Vector3.left;
        }

        public void SetSpeed(float speed)
        {
            // active = true;
            this.speed = speed;
        }

        public void SetActive(bool active) => this.active = active; 
    }
}
