using UnityEngine;

namespace AjaxNguyen.Core
{
    public class CloudSpawner : MonoBehaviour
    {
        #region Variables 
        [SerializeField] GameObject cloudPrefab;
        [SerializeField] Sprite[] cloudTextures;
        [SerializeField] float SPAWN_INTERVAL = 4f;
        [SerializeField] float prewarnDistance = 10f;
        [SerializeField] float Y_Deviation = 15f;     // độ lệch trục y khi spawn cloud

        [SerializeField] float minSpeed = 4f;
        [SerializeField] float maxSpeed = 7f;
        [SerializeField] float minScale = 1.2f;
        [SerializeField] float maxScale = 3f;

        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform destroyPoint;
        #endregion

        #region Unity callbacks
        private void Awake()
        {
            spawnPoint = transform;
        }

        private void Start()
        {
            PreWarm();
            Invoke(nameof(AttempSpawn), SPAWN_INTERVAL);
        }
        #endregion

        #region Other methods
        void SpawnCloud(float xPos)
        {
            // GameObject newCloud = Instantiate(cloudPrefab); 
            var newCloud = PoolManager.Instance.GetFromPool(cloudPrefab);

            float randY = Random.Range(spawnPoint.position.y - Y_Deviation, spawnPoint.position.y + Y_Deviation); // tạo tọa độ Y ngẫu nhiên cho cloud
            newCloud.transform.position = new Vector3(xPos, randY);

            float scale = Random.Range(minScale, maxScale);
            newCloud.transform.localScale = new Vector2(scale, scale);

            newCloud.GetComponent<SpriteRenderer>().sprite = cloudTextures[Random.Range(0, cloudTextures.Length)];
            newCloud.GetComponent<HorizontalMove>().SetUp(Random.Range(minSpeed, maxSpeed));
        }

        void AttempSpawn()
        {
            SpawnCloud(spawnPoint.position.x);
            Invoke("AttempSpawn", SPAWN_INTERVAL);
        }

        void PreWarm()
        {
            for (int i = 0; i < 10; i++)
            {
                SpawnCloud(spawnPoint.position.x - (i * prewarnDistance));
            }
        }

        #endregion
    }
}

