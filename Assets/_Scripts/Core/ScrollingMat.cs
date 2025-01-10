using UnityEngine;

namespace AjaxNguyen.Core
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ScrollingMat : MonoBehaviour
    {
        private MeshRenderer meshRenderer;
        [SerializeField] private float speed = 1f;
        [SerializeField] bool active = false;

        void Start()
        {
            Level.Instance.OnStateChange += Level_OnStateChange;
        }

        private void Level_OnStateChange(object sender, GameState e)
        {
            active = e == GameState.Playing;
        }

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        void Update()
        {
            if (!active) return;

            meshRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
        }

        public void SetSpeed(float speed) => this.speed = speed;

        public void SetActive(bool active) => this.active = active;

    }
}

