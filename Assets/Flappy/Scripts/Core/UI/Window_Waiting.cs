using UnityEngine;

namespace Flappy.Core.UI
{
    public class Window_Waiting : MonoBehaviour
    {
        void OnEnable()
        {
            Level.Instance.OnStateChange += TurnOffSelf;
        }

        private void OnDisable()
        {
            Level.Instance.OnStateChange -= TurnOffSelf;
        }

        private void TurnOffSelf(object sender, GameState e)
        {
            if (e == GameState.Playing)
            {
                gameObject.SetActive(false);
            }
        }
    }
}