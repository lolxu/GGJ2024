using System.Collections;
using UnityEngine;

namespace GAME_FILES.Scripts.PlayButton
{
    public class QuitButtonBehavior_PongIt : WorldButton
    {
        protected override void CustomStartBehavior()
        {
            _rigidbody.AddForce(new Vector2(1.0f, 0.0f) * 5.0f, ForceMode2D.Impulse);
        }

        protected override void CustomClickBehavior()
        {
            QuitGame();
        }

        public void StartQuit()
        {
            StopAllCoroutines();
            StartCoroutine(QuitSequence());
        }

        public void QuitGame()
        {
#if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE) 
            Application.Quit();
#elif (UNITY_WEBGL)
            Application.OpenURL("https://www.google.com/search?q=get+good&rlz=1C1CHZN_enUS1012US1012&oq=Get+good&gs_lcrp=EgZjaHJvbWUqBwgAEAAYgAQyBwgAEAAYgAQyBwgBEAAYgAQyCQgCEAAYChiABDIHCAMQLhiABDIHCAQQABiABDIHCAUQABiABDIHCAYQABiABDIHCAcQABiABDIHCAgQABiABDIHCAkQABiABNIBCDE0NzFqMGoxqAIAsAIA&sourceid=chrome&ie=UTF-8");
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            GameManager.Instance.PlayerLoses();
            LevelManager.Instance.LoadLevel("Menu");
#endif
        }
        
        IEnumerator QuitSequence()
        {
            Time.timeScale = 0.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            FeelMaster.Instance.PlayCamShake(2.0f);
            Debug.Log("Here");
            yield return new WaitForSecondsRealtime(0.5f);
            QuitGame();
        }
    }
}