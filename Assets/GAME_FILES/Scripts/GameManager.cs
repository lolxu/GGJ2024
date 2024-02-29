using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GAME_FILES.Scripts;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerLives = 3;
    public bool gameEnds = false;
    public int winningScore = 0;
    public GameTitleScript gameTitle;
    public GameTitleScript gameScore;
    public GameTitleScript gameLives;
    public bool isScoreBased = true;
    public string gameName;
    
    [Header("Timer Based Settings")]
    public bool isTimeBased = false;
    public float timeLeft = 0.0f;
    public GameTitleScript gameTime;
    private bool timerCanStart = false;

    public Action OnPlayerWins;
    public Action OnPlayerLoses;

    private GameObject playButton;
    private Vector3 buttonStartPos;
    private int score = 0;
    private int lives;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        lives = playerLives;
        playButton = GameObject.FindGameObjectWithTag("StartButton");

    }

    private void Start()
    {
        // For idle games
        if (!playButton)
        {
            BeginGame();
        }
    }

    public void BeginGame()
    {
        if (gameTitle)
        {
            gameTitle.ChangeText(gameName);
        }
        if (gameScore)
        {
            gameScore.ChangeText(score + " / " + winningScore);
        }
        if (gameLives)
        {
            gameLives.ChangeText(lives + " / " + playerLives);
        }

        if (EnemySpawner.Instance)
        {
            EnemySpawner.Instance.SpawnEnemies();
        }

        timerCanStart = true;
    }

    public void Update()
    {
        if (isTimeBased && !gameEnds && timerCanStart)
        {
            if (timeLeft >= 0.0f)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                timeLeft = 0.0f;
                PlayerWins();
            }

            if (gameTime)
            {
                int minutes = Mathf.FloorToInt(timeLeft / 60F);
                int seconds = Mathf.FloorToInt(timeLeft - minutes * 60);

                string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
                gameTime.ChangeText(niceTime);
            }
        }
    }

    public void DecreaseLife(int amount)
    {
        lives -= amount;
        if (lives <= 0 && !gameEnds)
        {
            lives = 0;
            PlayerLoses();
        }
        if (gameLives)
        {
            gameLives.ChangeText(lives + " / " + playerLives);
        }
        else
        {
            Debug.LogError("Game Lives Text Not Set");
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        if (score >= winningScore && !gameEnds)
        {
            score = winningScore;
            PlayerWins();
        }
        if (gameScore)
        {
            gameScore.ChangeText(score + " / " + winningScore);
        }
        else
        {
            Debug.LogError("Game Lives Text Not Set");
        }
    }

    public void PlayerWins()
    {
        gameEnds = true;
        OnPlayerWins?.Invoke();
        gameTitle.ChangeText("GOOD JOB");
        
        LevelManager.Instance.ChangeLevelCompletionStatus(SceneManager.GetActiveScene().name);

        if (!playButton)
        {
            StartCoroutine(LoadToMenu());
        }
    }

    IEnumerator LoadToMenu()
    {
        yield return new WaitForSeconds(1.5f);
        LevelManager.Instance.LoadLevel("Menu");
    }

    public void PlayerLoses()
    {
        gameEnds = true;
        gameTitle.ChangeText("BAD JOB");
        OnPlayerLoses?.Invoke();
    }

}
