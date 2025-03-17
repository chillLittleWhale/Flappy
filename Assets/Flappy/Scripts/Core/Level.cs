using System;
using Flappy.Core.Manager;
using UnityEngine;
using AjaxNguyen.Event;

namespace Flappy.Core
{
    public class Level : MonoBehaviour
    {
        #region Variables and Properties
        public static Level Instance;

        private const float CAM_OTHOR_SIZE = 50f;
        private const float GROUND_HEIGHT = 15f;
        private const float MIN_HEIGHT_TO_EDGE = 5f;

        [SerializeField] private PipeSpawner pipeSpawner;
        // [SerializeField] private Transform startPoint;
        // [SerializeField] private GameObject pipes;

        [SerializeField] float pipeInterval = 1.8f;
        [SerializeField] float Y_move_rate = 0f;
        [SerializeField] float gapSize = 30f;
        [SerializeField] float pipe_y_speed = 2f;
        [SerializeField] float pipe_x_speed = 10f;


        private float timer = 0f;
        private int spawnedPipes = 0;
        private int playerScore = 0;

        private GameState state;
        private bool canVerticalMove;
        private float ySpawnPos;
        #endregion

        #region Events
        public event EventHandler<int> OnScoreChanged;
        public event EventHandler<GameState> OnStateChange;
        [SerializeField] IntEventChanel OnRecordBreak;
        [SerializeField] IntEventChanel OnScoreChangedChanel;

        // public void Level_OnPlayerScore(object sender, System.EventArgs e)
        // {
        //     playerScore += 1;
        //     OnScoreChanged?.Invoke(this, playerScore);
        // }

        // public void Level_OnPlayerDeath(object sender, System.EventArgs e)
        // {
        //     ChangeState(GameState.GameOver);
        //     TrySetHighestScore(playerScore);
        // }

        public void Level_OnPlayerScore()
        {
            playerScore += 1;
            // OnScoreChanged?.Invoke(this, playerScore);
            OnScoreChangedChanel.Raise(playerScore);
        }

        public void Level_OnPlayerDeath()
        {
            ChangeState(GameState.GameOver);
            TrySetHighestScore(playerScore);
        }
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            Instance = this;

            state = GameState.Waiting;
        }

        void Start()
        {
            MapManager.Instance.LoadMap();

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
                    // state = GameState.Playing;
                    // OnStateChange?.Invoke(this, state);
                    ChangeState(GameState.Playing);
                }
            }
        }

        // void OnEnable()
        // {
        //     // Player.Instance.OnPlayerScore += Level_OnPlayerScore;
        //     // Player.Instance.OnPlayerDeath += Level_OnPlayerDeath;


        // }

        // void OnDisable()
        // {
        //     // Player.Instance.OnPlayerScore -= Level_OnPlayerScore;
        //     // Player.Instance.OnPlayerDeath -= Level_OnPlayerDeath;
        // }
        #endregion

        #region Other methods
        private void CreatePipes()
        {
            ySpawnPos = UnityEngine.Random.Range(-CAM_OTHOR_SIZE + gapSize * 0.5f + GROUND_HEIGHT + MIN_HEIGHT_TO_EDGE, CAM_OTHOR_SIZE - gapSize * 0.5f - MIN_HEIGHT_TO_EDGE);
            canVerticalMove = UnityEngine.Random.Range(0f, 100f) < Y_move_rate;

            var minHeight = -CAM_OTHOR_SIZE + gapSize * 0.5f + MIN_HEIGHT_TO_EDGE + GROUND_HEIGHT;
            var maxHeight = CAM_OTHOR_SIZE - gapSize * 0.5f - MIN_HEIGHT_TO_EDGE;

            pipeSpawner.CreatePipes(gapSize, ySpawnPos, canVerticalMove, pipe_x_speed, pipe_y_speed, minHeight, maxHeight);
        }

        public GameState GetState() => state;

        private void ChangeState(GameState newState)
        {
            if (state == newState) return;

            state = newState;
            OnStateChange?.Invoke(this, state);
        }

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
                    gapSize = 32f;
                    pipeInterval = 3f;
                    Y_move_rate = 0f;
                    break;
                case Difficulty.Medium:
                    gapSize = 25f;
                    pipeInterval = 2.8f;
                    Y_move_rate = 25f;
                    pipe_y_speed = 3f;
                    break;
                case Difficulty.Hard:
                    gapSize = 20f;
                    pipeInterval = 2f;
                    Y_move_rate = 50f;
                    pipe_y_speed = 10f;
                    break;
                case Difficulty.Extreme:
                    gapSize = 16f;
                    pipeInterval = 1.8f;
                    Y_move_rate = 100f;
                    pipe_y_speed = 30f;
                    break;
            }
        }

        public int GetHighestScore() => PlayerPrefs.GetInt("HighestScore", 0);

        public void TrySetHighestScore(int score)
        {
            int CurrentHighestScore = PlayerPrefs.GetInt("HighestScore", 0);
            if (CurrentHighestScore < score)
            {
                PlayerPrefs.SetInt("HighestScore", score);
                OnRecordBreak.Raise(score);
            }
        }
        #endregion
    }

    public enum GameState
    {
        Waiting, Playing, GameOver
    }

    public enum Difficulty
    {
        Easy, Medium, Hard, Extreme
    }
}