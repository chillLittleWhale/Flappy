using System;
using AjaxNguyen.Core.Manager;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private GameState levelState;

    [SerializeField] float minFlapForce = 75f;
    [SerializeField] float maxFlapForce = 100f;

    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerScore;


    void Awake()
    {
        instance = this;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Static;
    }

    void Start()
    {
        Level.GetInstance().OnStateChange += Player_OnStateChange;
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
                transform.eulerAngles = new Vector3(0f, 0f, rb.velocity.y * 0.1f);   // phai de o vi tri nay
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
            OnPlayerDeath?.Invoke(instance, EventArgs.Empty);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnPlayerScore?.Invoke(instance, EventArgs.Empty);
    }

    public static Player GetInstance() => instance;

    private void Player_OnStateChange (object sender, GameState e )
    {
        levelState = Level.GetInstance().GetState();
        rb.bodyType = levelState == GameState.Playing ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
    }
    private void HandleFlap()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // chuột trái
        {
            MinFlap();

            ResourceManager.Instance.AddResource(ResourceType.Stamina, 1);  //TODO
        }
        else if (Input.GetKeyDown(KeyCode.M) || Input.GetMouseButtonDown(1)) // chuột phải
        {
            MaxFlap();
        }
    }

    private void MinFlap()
    {
        rb.velocity = Vector2.up * minFlapForce;
    }

    private void MaxFlap()
    {
        rb.velocity = Vector2.up * maxFlapForce;
    }

}
