using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GAME_FILES.Scripts.PlayButton;
using UnityEngine;
using Random = UnityEngine.Random;

public class PongBall : MonoBehaviour
{
    // private Rigidbody2D _rigidbody;

    public float speed = 10.0f;
    private Vector3 moveDirection = Vector3.down;
    private Rigidbody2D _rigidbody;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            var initDir = new Vector2(Random.Range(-0.5f, 0.5f), -1.0f);
            _rigidbody.AddForce(initDir.normalized * 10.0f, ForceMode2D.Impulse);
        });
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collided");
        if (!GameManager.Instance.gameEnds)
        {
            if (other.collider.CompareTag("StartButton"))
            {
                FeelMaster.Instance.PlayCamShake(0.75f);
                GameManager.Instance.IncreaseScore(1);
            }
            else if (other.collider.CompareTag("QuitButton"))
            {
                var script = other.gameObject.GetComponent<QuitButtonBehavior_PongIt>();
                script.StartQuit();
            }
            else
            {
                _rigidbody.AddForce(Random.insideUnitCircle.normalized * 7.0f, ForceMode2D.Impulse);
            }
        }
        
        FeelMaster.Instance.PlayPongHit(gameObject.transform.position, 1.0f);
        FeelMaster.Instance.PlayCamShake(0.3f);
    }

    private void OnBecameInvisible()
    {
        if (!GameManager.Instance.gameEnds && BallSpawner.Instance)
        {
            GameManager.Instance.DecreaseLife(1);
            BallSpawner.Instance.SpawnBall();
            Destroy(gameObject);
        }
    }
}
