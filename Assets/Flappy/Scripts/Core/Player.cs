using System;
using Flappy.Core.Manager;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Flappy.Core
{
    public class Player : MonoBehaviour
    {
        #region Variables
        public static Player Instance;
        private Rigidbody2D rb;
        private SpriteLibrary spriteLibrary;
        private GameState levelState;

        [SerializeField] float minFlapForce = 75f;
        [SerializeField] float maxFlapForce = 100f;

        #endregion

        #region Events
        public event EventHandler OnPlayerDeath;
        public event EventHandler OnPlayerScore;

        #endregion

        #region Unity Callbacks
        void Awake()
        {
            Instance = this;
            spriteLibrary = GetComponent<SpriteLibrary>();
            rb = GetComponent<Rigidbody2D>();

            rb.bodyType = RigidbodyType2D.Static;
        }

        void Start()
        {
            Level.Instance.OnStateChange += Player_OnStateChange;
            spriteLibrary.spriteLibraryAsset = SkinManager.Instance.GetCurrentSpriteLibraryAsset();
        }

        void Update()
        {
            switch (levelState)
            {
                case GameState.Waiting:
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.M) || Input.GetMouseButtonDown(1))
                    {
                        rb.bodyType = RigidbodyType2D.Dynamic;
                        HandleFlap();
                    }
                    break;
                case GameState.Playing:
                    HandleFlap();
                    transform.eulerAngles = new Vector3(0f, 0f, rb.linearVelocity.y * 0.1f);   // phải để dưới HandleFlap
                    break;
                case GameState.GameOver:
                    break;
            }

        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("ForeGround") || other.gameObject.CompareTag("Pipe"))
            {
                rb.bodyType = RigidbodyType2D.Static;
                OnPlayerDeath?.Invoke(Instance, EventArgs.Empty);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnPlayerScore?.Invoke(Instance, EventArgs.Empty);
            ResourceManager.Instance.AddResource(RewardType.Gold, 1);  //TODO
        }

        #endregion

        #region Other Methods
        private void Player_OnStateChange(object sender, GameState e)
        {
            levelState = Level.Instance.GetState();
            rb.bodyType = levelState == GameState.Playing ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
        }

        private void HandleFlap()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // chuột trái
            {
                MinFlap();
            }
            else if (Input.GetKeyDown(KeyCode.M) || Input.GetMouseButtonDown(1)) // chuột phải
            {
                MaxFlap();
            }
        }

        private void MinFlap()
        {
            rb.linearVelocity = Vector2.up * minFlapForce;
        }

        private void MaxFlap()
        {
            rb.linearVelocity = Vector2.up * maxFlapForce;
        }

        #endregion
    }
}