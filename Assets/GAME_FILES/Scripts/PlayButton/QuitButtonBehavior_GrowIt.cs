using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GAME_FILES.Scripts.PlayButton
{
    public class QuitButtonBehavior_GrowIt : WorldButton
    {
        private bool hasReachedFinish = false;
        protected override void CustomStartBehavior()
        {
            StartCoroutine(GrowSequence());
        }

        private IEnumerator GrowSequence()
        {
            yield return new WaitForSeconds(0.25f);
            while (!hasReachedFinish)
            {
                transform.position = new Vector3(transform.position.x + Random.Range(1.5f, 3.0f), transform.position.y, transform.position.z);
                yield return new WaitForSeconds(Random.Range(0.25f, 0.35f));
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Finish"))
            {
                hasReachedFinish = true;
                StopCoroutine(GrowSequence());
                
                GameManager.Instance.PlayerLoses();
            }
        }

        protected override void WinBehavior()
        {
            hasReachedFinish = true;
            StopCoroutine(GrowSequence());
        }

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
            Application.OpenURL("https://www.google.com/search?q=get+good&rlz=1C1CHZN_enUS1012US1012&oq=Get+good&gs_lcrp=EgZjaHJvbWUqBwgAEAAYgAQyBwgAEAAYgAQyBwgBEAAYgAQyCQgCEAAYChiABDIHCAMQLhiABDIHCAQQABiABDIHCAUQABiABDIHCAYQABiABDIHCAcQABiABDIHCAgQABiABDIHCAkQABiABNIBCDE0NzFqMGoxqAIAsAIA&sourceid=chrome&ie=UTF-8");
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            GameManager.Instance.PlayerLoses();
            LevelManager.Instance.LoadLevel("Menu");
#endif
        }
    }
}