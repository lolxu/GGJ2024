using UnityEngine;

namespace GAME_FILES.Scripts.PlayButton
{
    public class QuitButtonBehavior_End : WorldButton
    {
        protected override void CustomClickBehavior()
        {
            QuitGame();
        }

        public void QuitGame()
        {
#if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE) 
            Application.Quit();
#elif (UNITY_WEBGL)
            Application.OpenURL("https://media.tenor.com/_Un-0VOl1P8AAAAe/gold-star-good-job.png");
#endif
        }
    }
}