using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GAME_FILES.Scripts.PlayButton;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonBehaviorLevel_MoveIt : WorldButton
{
    private bool isFirstHover = false;
    private Vector3 buttonStartPos; 
    
    private float derTimeScale = 1.0f;
    private float derSlowScale = 0.05f;

    private bool hasStopped = false;
    
    protected override void CustomStartBehavior()
    {
        buttonStartPos = transform.position;
        GameManager.Instance.BeginGame();
    }

    protected override void CustomUpdateBehavior()
    {
        if (!GameManager.Instance.gameEnds && !hasStopped)
        {
            Time.timeScale = GameControl.Instance.isMovingStartButton ? derTimeScale : derSlowScale;

            Debug.Log(Time.timeScale);
        
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }

    protected override void WinBehavior()
    {
        Debug.Log(buttonStartPos);
        StartCoroutine(ResetSequence());
    }

    protected override void LoseBehavior()
    {
        Debug.Log("Loses");
        Time.timeScale = derTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        StartCoroutine(ResetSequence());
    }

    IEnumerator ResetSequence()
    {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0.0f;
        _rigidbody.gravityScale = 0.0f;
        _rigidbody.isKinematic = true;

        yield return new WaitForSeconds(0.1f);

        transform.DOMove(buttonStartPos, 1.25f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                LevelManager.Instance.LoadLevel("Menu");
            });
        });
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("QuitButton"))
        {
            var script = other.gameObject.GetComponent<QuitButtonBehavior>();
            script.StartQuit();
        }
    }
}
