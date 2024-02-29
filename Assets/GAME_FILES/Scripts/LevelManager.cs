using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [Serializable]
    public class LevelMaster
    {
        public List<string> levelNames;
        public List<bool> levelCompletion;
        public List<bool> needsTransition;
    }
    public LevelMaster levelMaster;
    [SerializeField] private Animator transitionAnim;
    public AudioSource audioSource;
    public AudioSource musicSource;

    private int levelsLeft;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        levelsLeft = levelMaster.levelNames.Count;
        
        DontDestroyOnLoad(gameObject);
    }

    public string ChooseARandomUncompletedLevel()
    {
        int counter = 0;
        foreach (var stat in levelMaster.levelCompletion)
        {
            if (!stat)
            {
                counter++;
            }
        }
        
        if (counter > 0)
        {
            int ind = Random.Range(0, levelMaster.levelNames.Count);
            do
            {
                ind = Random.Range(0, levelMaster.levelNames.Count);
            } while (levelMaster.levelCompletion[ind]);
            return levelMaster.levelNames[ind];
        }
        else
        {
            FeelMaster.Instance.PlayCamShake(3.0f);
            musicSource.Stop();
            LoadLevel("Level_Quit");
        }
        return "";
    }

    /// <summary>
    /// I am sorry about this stupid function
    /// </summary>
    /// <returns></returns>
    public int CountCompletedLevels()
    {
        int ans = 0;
        foreach (var stat in levelMaster.levelCompletion)
        {
            if (stat)
            {
                ans++;
            }
        }

        return ans;
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelSequence(levelName));
    }

    IEnumerator LoadLevelSequence(string levelName)
    {
        var index = levelMaster.levelNames.IndexOf(levelName);
        if (index != -1)
        {
            if (levelMaster.needsTransition[index])
            {
                transitionAnim.SetTrigger("End");
                yield return new WaitForSeconds(1.0f);
                SceneManager.LoadSceneAsync(levelName);
                transitionAnim.SetTrigger("Start");
            }
            else
            {
                SceneManager.LoadSceneAsync(levelName);
            }
        }
        else
        {
            var index2 = levelMaster.levelNames.IndexOf(SceneManager.GetActiveScene().name);
            if (index2 != -1)
            {
                if (levelMaster.needsTransition[index2])
                {
                    transitionAnim.SetTrigger("End");
                    yield return new WaitForSeconds(1.0f);
                    SceneManager.LoadSceneAsync(levelName);
                    transitionAnim.SetTrigger("Start");
                }
                else
                {
                    SceneManager.LoadSceneAsync(levelName);
                }
            }
            else
            {
                SceneManager.LoadSceneAsync(levelName);
            }
        }
    }

    public void ChangeLevelCompletionStatus(string levelName)
    {
        var index = levelMaster.levelNames.IndexOf(levelName);
        if (index != -1)
        {
            levelMaster.levelCompletion[index] = true;
            levelsLeft--;
        }
    }
}
