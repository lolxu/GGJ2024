using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonBehaviorLevel_CatchIt : WorldButton
{
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
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(Random.insideUnitCircle * Random.Range(40.0f, 80.0f), ForceMode2D.Impulse);
        }
    }

    protected override void CustomClickBehavior()
    {
        if (!GameManager.Instance.gameEnds)
        {
            FeelMaster.Instance.PlayCamShake(1.0f);
            FeelMaster.Instance.PlayPlayerHit(gameObject.transform.position, 1.0f);
            GameManager.Instance.IncreaseScore(1);
        }
    }

    protected override void WinBehavior()
    {
        Debug.Log(buttonStartPos);

        StartCoroutine(ResetSequence());
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
