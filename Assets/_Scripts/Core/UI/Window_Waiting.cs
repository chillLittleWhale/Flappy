using UnityEngine;

public class Window_Waiting : MonoBehaviour
{
    void OnEnable()
    {
        Level.GetInstance().OnStateChange += TurnOffSelf;
    }

    private void OnDisable()
    {
        Level.GetInstance().OnStateChange -= TurnOffSelf;
    }

    private void TurnOffSelf(object sender, GameState e)
    {
        if (e == GameState.Playing)
        {
            gameObject.SetActive(false);
        }
    }
}
