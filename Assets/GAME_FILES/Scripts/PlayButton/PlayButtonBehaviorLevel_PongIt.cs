using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonBehaviorLevel_PongIt : WorldButton
{
    private bool isFirstHover = false;
    private Vector3 buttonStartPos; 
    protected override void CustomStartBehavior()
    {
        buttonStartPos = transform.position;
        
        GameManager.Instance.BeginGame();
    }

    protected override void WinBehavior()
    {
        Debug.Log(buttonStartPos);
        StartCoroutine(ResetSequence());
    }

    protected override void LoseBehavior()
    {
        Debug.Log("Loses");
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
}
