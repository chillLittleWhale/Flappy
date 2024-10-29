using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private GameObject pipes;

    private const float CAM_OTHOR_SIZE = 50f;
    private const float PIPE_BODY_WIDTH = 8f;   // độ rộng thân ống
    private const float PIPE_HEAD_HEIGHT = 3f;  // độ cao đầu ống

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

    private bool canHorirontalMove;
    private float ySpawnPos;

    void Start()
    {
        SetDifficulty(Difficulty.Easy);
    }

    void Update()
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

    private void CreatePipes()
    {
        ySpawnPos = Random.Range(-CAM_OTHOR_SIZE + gapSize * 0.5f + GROUND_HEIGHT + MIN_HEIGHT_TO_EDGE, CAM_OTHOR_SIZE - gapSize * 0.5f - MIN_HEIGHT_TO_EDGE);
        canHorirontalMove = Random.Range(0f, 100f) < Y_move_rate;                      

        // var newPipe = Instantiate(pipes, startPoint.position, Quaternion.identity);
        var newPipe = PoolManager.Instance.GetFromPool(pipes.gameObject);
        newPipe.transform.position = startPoint.position;

        var pipesController = newPipe.GetComponents<PipesController>();
        pipesController[0].SetupPipe(gapSize, ySpawnPos, canHorirontalMove, pipe_y_speed,
            CAM_OTHOR_SIZE - gapSize * 0.5f - MIN_HEIGHT_TO_EDGE,
            -CAM_OTHOR_SIZE + gapSize * 0.5f + MIN_HEIGHT_TO_EDGE + GROUND_HEIGHT);

        newPipe.GetComponent<HorizontalMove>().SetUp(pipe_x_speed);
    }

    private enum Difficulty
    {
        Easy, Medium, Hard, Extreme
    }

    private Difficulty GetDifficulty()
    {
        if (spawnedPipes >= 9) return Difficulty.Extreme;
        if (spawnedPipes >= 6) return Difficulty.Hard;
        if (spawnedPipes >= 3) return Difficulty.Medium;
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
                pipeInterval = .5f;
                Y_move_rate = 100f;
                pipe_y_speed = 30f;
                break;
        }
    }
}
