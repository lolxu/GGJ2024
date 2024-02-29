using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayButtonBehaviorLevel_GrowIt : WorldButton
{
    public float walkAmount = 1.5f;
    private bool isFirstHover = false;
    private Vector3 buttonStartPos; 
    protected override void CustomStartBehavior()
    {
        buttonStartPos = transform.position;
        needsClickAnimation = false;
        GameManager.Instance.BeginGame();
    }

    protected override void CustomHoverBehavior()
    {
        if (!GameManager.Instance.gameEnds)
        {
            if (!isFirstHover)
            {
                isFirstHover = true;
            }
        }
    }

    protected override void CustomClickBehavior()
    {
        if (!GameManager.Instance.gameEnds)
        {
            FeelMaster.Instance.PlayCamShake(0.5f);
            FeelMaster.Instance.PlayPlayerHit(gameObject.transform.position, 0.5f);
            transform.position = new Vector3(transform.position.x + walkAmount, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.CompareTag("Finish"))
        {
            GameManager.Instance.PlayerWins();
        }
    }

    protected override void WinBehavior()
    {
        LevelManager.Instance.LoadLevel("Menu");
        // StartCoroutine(ResetSequence());
    }

    protected override void LoseBehavior()
    {
        LevelManager.Instance.LoadLevel("Menu");
    }

    IEnumerator ResetSequence()
    {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0.0f;
        _rigidbody.gravityScale = 0.0f;
        _rigidbody.isKinematic = true;

        yield return new WaitForSeconds(0.1f);

        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(transform.DOMove(buttonStartPos, 1.25f).SetEase(Ease.InOutSine));
        mySequence.Append(transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.InOutSine));
        mySequence.AppendInterval(0.5f);
        
        mySequence.Play().OnComplete(() =>
        {
            LevelManager.Instance.LoadLevel("Menu");
        });
    }
}
