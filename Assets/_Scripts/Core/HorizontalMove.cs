using UnityEngine;

namespace AjaxNguyen.Core
{
    public class HorizontalMove : MonoBehaviour
    {
        private float speed;
        private bool started = false;

        void Update()
        {
            if (started)
            {
                transform.position += speed * Time.deltaTime * Vector3.left;
            }
        }

        public void SetUp(float speed)
        {
            started = true;
            this.speed = speed;
        }
    }
}
