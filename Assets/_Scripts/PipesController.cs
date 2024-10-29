using UnityEngine;

public class PipesController : MonoBehaviour
{
    [SerializeField] private Transform topPipe;      //  pipe trên
    [SerializeField] private Transform bottomPipe;   // pipe dưới
    [SerializeField] private Pipes_Y_Move pipeYMove;


    void Awake()
    {
        pipeYMove = GetComponent<Pipes_Y_Move>();
    }
    
    public void SetupPipe( float gapSize, float gapYPos, bool isVeticalMove, float ySpeed = 0f, float maxHeight = 0f, float minHeight = 0f) // Hàm để căn chỉnh lại cặp pipe
    {
        if (isVeticalMove) 
        {
            pipeYMove.enabled = true;   // mặc định thì script này sẽ bị tắt đi
            pipeYMove.SetUpYMove(ySpeed, maxHeight, minHeight);
        }

        transform.position = new Vector2(transform.position.x, gapYPos);
        topPipe.localPosition = new Vector2(0f,  gapSize * 0.5f);
        bottomPipe.localPosition = new Vector2(0f,  -gapSize * 0.5f);
    }
}
