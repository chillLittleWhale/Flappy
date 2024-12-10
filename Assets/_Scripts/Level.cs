using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    private static Level instance;

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private GameObject pipes;

    private GameState state;
    private const float CAM_OTHOR_SIZE = 50f;
    // private const float PIPE_BODY_WIDTH = 8f;   // độ rộng thân ống
    // private const float PIPE_HEAD_HEIGHT = 3f;  // độ cao đầu ống

    // private const float MIN_GAP = 10f;
    // private const float MAX_GAP = 40f;
    private const float GROUND_HEIGHT = 15f;
    private const float MIN_HEIGHT_TO_EDGE = 5f;

    [SerializeField] float pipeInterval = 1.8f;
    [SerializeField] float Y_move_rate = 0f;
    [SerializeField] float gapSize = 30f;
    [SerializeField] float pipe_y_speed = 2f;
    [SerializeField] float pipe_x_speed = 10f;


    private float timer = 0f;
    private int spawnedPipes = 0;
    private int playerScore = 0;

    private bool canHorirontalMove;
    private float ySpawnPos;

    public event EventHandler<int> OnScoreChanged;
    public event EventHandler<GameState> OnStateChange;

    private void Level_OnPlayerScore(object sender, System.EventArgs e)
    {
        playerScore++;
        OnScoreChanged?.Invoke(this, playerScore);
    }

    private void Level_OnPlayerDeath(object sender, System.EventArgs e)
    {
        state = GameState.GameOver;
        OnStateChange?.Invoke(this, state);
    }

    void Awake()
    {
        instance = this;

        state = GameState.Waiting;
    }

    void Start()
    {
        SetDifficulty(Difficulty.Easy);
        timer = pipeInterval;
    }

    void Update()
    {
        if (state == GameState.Playing)
        {
            timer += Time.deltaTime;
            if (timer > pipeInterval)
            {
                timer = 0f;

                CreatePipes();
                spawnedPipes++;
                SetDifficulty(GetDifficulty());
            }
        }

        else if (state == GameState.Waiting)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.M) || Input.GetMouseButtonDown(1))
            {
                state = GameState.Playing;
                OnStateChange?.Invoke(this, state);
            }
        }
    }

    void OnEnable()
    {
        Player.GetInstance().OnPlayerScore += Level_OnPlayerScore;
        Player.GetInstance().OnPlayerDeath += Level_OnPlayerDeath;
    }

    void OnDisable()
    {
        Player.GetInstance().OnPlayerScore -= Level_OnPlayerScore;
        Player.GetInstance().OnPlayerDeath -= Level_OnPlayerDeath;
    }

    public static Level GetInstance() => instance;


    private void CreatePipes()
    {
        ySpawnPos = UnityEngine.Random.Range(-CAM_OTHOR_SIZE + gapSize * 0.5f + GROUND_HEIGHT + MIN_HEIGHT_TO_EDGE, CAM_OTHOR_SIZE - gapSize * 0.5f - MIN_HEIGHT_TO_EDGE);
        canHorirontalMove = UnityEngine.Random.Range(0f, 100f) < Y_move_rate;

        // var newPipe = Instantiate(pipes, startPoint.position, Quaternion.identity);
        var newPipe = PoolManager.Instance.GetFromPool(pipes.gameObject);
        newPipe.transform.position = startPoint.position;

        var pipesController = newPipe.GetComponents<PipesController>();
        pipesController[0].SetupPipe(gapSize, ySpawnPos, canHorirontalMove, pipe_y_speed,
            CAM_OTHOR_SIZE - gapSize * 0.5f - MIN_HEIGHT_TO_EDGE,
            -CAM_OTHOR_SIZE + gapSize * 0.5f + MIN_HEIGHT_TO_EDGE + GROUND_HEIGHT);

        newPipe.GetComponent<HorizontalMove>().SetUp(pipe_x_speed);
    }

    public GameState GetState() => state;

    public int GetPlayerScore() => playerScore;

    private Difficulty GetDifficulty()
    {
        if (spawnedPipes >= 31) return Difficulty.Extreme;
        if (spawnedPipes >= 21) return Difficulty.Hard;
        if (spawnedPipes >= 11) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    private void SetDifficulty(Difficulty difficulty)  // TODO: cho vào scriptable object 
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 40f;
                pipeInterval = 2f;
                Y_move_rate = 0f;
                break;
            case Difficulty.Medium:
                gapSize = 25f;
                pipeInterval = 1.8f;
                Y_move_rate = 25f;
                pipe_y_speed = 3f;
                break;
            case Difficulty.Hard:
                gapSize = 20f;
                pipeInterval = 1.6f;
                Y_move_rate = 50f;
                pipe_y_speed = 10f;
                break;
            case Difficulty.Extreme:
                gapSize = 15f;
                pipeInterval = 1.8f;
                Y_move_rate = 100f;
                pipe_y_speed = 30f;
                break;
        }
    }
}

public enum GameState
{
    Waiting, Playing, GameOver
}

public enum Difficulty
{
    Easy, Medium, Hard, Extreme
}
