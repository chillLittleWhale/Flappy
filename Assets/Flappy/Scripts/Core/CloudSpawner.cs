using System.Collections.Generic;
using System.Linq;
using AjaxNguyen.Utility.ObjectPooling;
using UnityEngine;

namespace Flappy.Core
{
    public class CloudSpawner : MonoBehaviour
    {
        #region Variables 
        [SerializeField] GameObject cloudPrefab;
        [SerializeField] Sprite[] cloudTextures;

        [SerializeField] bool active = false;
        [SerializeField] float SPAWN_INTERVAL = 4f;
        [SerializeField] float Y_Deviation = 15f;     // độ lệch trục y khi spawn cloud (lúc spawn thì object sẽ cao hoặc thấp hơn spawnPoint.y 1 khoảng tối đa giá trị này)
        [SerializeField] int prewarmObjects = 10;
        [SerializeField] float prewarmDistance = 10f;

        [SerializeField] float minSpeed = 4f;
        [SerializeField] float maxSpeed = 7f;
        [SerializeField] float minScale = 1.2f;
        [SerializeField] float maxScale = 3f;
        private float timer = 0f;

        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform destroyPoint;
        #endregion

        #region Unity callbacks
        private void Start()
        {
            if (spawnPoint == null) spawnPoint = transform;

            PreWarm();
            // Invoke(nameof(AttempSpawn), SPAWN_INTERVAL);

            Level.Instance.OnStateChange += Level_OnStateChange;
        }

        void Update()
        {
            if (!active) return;

            timer += Time.deltaTime;
            if (timer >= SPAWN_INTERVAL)
            {
                timer = 0f; 
                AttempSpawn(); 
            }
        }
        #endregion

        #region Other methods
        private void Level_OnStateChange(object sender, GameState e)
        {
            active = e == GameState.Playing;
            List<HorizontalMove> horizontalComps = transform.GetComponentsInChildren<HorizontalMove>().ToList();

            horizontalComps.ForEach(comp =>
            {
                comp.SetActive(active);
            });
        }

        void SpawnCloud(float xPos)
        {
            var newCloud = PoolManager.Instance.GetFromPool(cloudPrefab);

            float randY = Random.Range(spawnPoint.position.y - Y_Deviation, spawnPoint.position.y + Y_Deviation); // tạo tọa độ Y ngẫu nhiên cho cloud
            newCloud.transform.position = new Vector3(xPos, randY);

            float scale = Random.Range(minScale, maxScale);
            newCloud.transform.localScale = new Vector2(scale, scale);

            newCloud.GetComponent<SpriteRenderer>().sprite = cloudTextures[Random.Range(0, cloudTextures.Length)];
            newCloud.GetComponent<HorizontalMove>().SetSpeed(Random.Range(minSpeed, maxSpeed));
            // newCloud.GetComponent<HorizontalMove>().SetActive(true);

            newCloud.transform.parent = transform;
        }

        void AttempSpawn()
        {
            SpawnCloud(spawnPoint.position.x);
            // Invoke("AttempSpawn", SPAWN_INTERVAL);
        }

        void PreWarm()
        {
            for (int i = 0; i < prewarmObjects; i++)
            {
                SpawnCloud(spawnPoint.position.x - (i * prewarmDistance));
            }
        }

        #endregion
    }
}

