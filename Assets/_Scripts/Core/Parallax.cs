using UnityEngine;

namespace AjaxNguyen.Core
{
    public class Parallax : MonoBehaviour
    {
        private MeshRenderer meshRenderer;
        [SerializeField] private float speed = 1f;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        void Update()
        {
            meshRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

    }
}

