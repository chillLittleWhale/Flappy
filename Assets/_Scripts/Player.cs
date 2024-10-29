using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private State state;

    [SerializeField] float minFlapForce = 75f;
    [SerializeField] float maxFlapForce = 100f;

    public event EventHandler OnDied;
    public event EventHandler OnStartPlaying;

    private enum State
    {
        Waiting,
        Playing,
        Died
    }

    void Awake()
    {
        instance = this;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Static;
        state = State.Waiting;
    }

    void Start()
    {

    }

    void Update()
    {
        switch (state)
        {
            case State.Waiting:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.M) || Input.GetMouseButtonDown(1))
                {
                    state = State.Playing;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    OnStartPlaying?.Invoke(this, EventArgs.Empty);

                    HandleFlap();
                }
                break;
            case State.Playing:
                    HandleFlap();
                transform.eulerAngles = new Vector3(0f, 0f, rb.velocity.y * 0.1f);   // phai de o vi tri nay
                break;
            case State.Died:
                break;
        }

    }

    public static Player GetInstance()
    {
        return instance;
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
        rb.velocity = Vector2.up * minFlapForce;
    }

    private void MaxFlap()
    {
        rb.velocity = Vector2.up * maxFlapForce;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ForeGround") || other.gameObject.CompareTag("Pipe"))
        state = State.Died;
        rb.bodyType = RigidbodyType2D.Static;
        // SoundManager.PlaySound(SoundManager.Sound.Lose, true);
        if (OnDied != null) OnDied(instance, EventArgs.Empty);
    }

}
