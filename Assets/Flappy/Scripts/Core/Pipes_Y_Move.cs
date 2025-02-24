using UnityEngine;

namespace Flappy.Core
{
    public class Pipes_Y_Move : MonoBehaviour
    {
        [SerializeField] float speed = 3f;
        [SerializeField] float maxHeight = 50f;
        [SerializeField] float minHeight = -35f;
        [SerializeField] int direction;  // 1: up, -1: down

        void Start()
        {
            direction = Random.Range(0, 2) == 1 ? 1 : -1;
        }

        void Update()
        {
            if (transform.position.y > maxHeight || transform.position.y < minHeight)
            {
                direction *= -1;
            }
            transform.position += direction * speed * Time.deltaTime * Vector3.up;
        }

        public void SetUpYMove(float speed, float maxHeight, float minHeight)
        {
            this.speed = speed;
            this.maxHeight = maxHeight;
            this.minHeight = minHeight;
        }
    }
}
