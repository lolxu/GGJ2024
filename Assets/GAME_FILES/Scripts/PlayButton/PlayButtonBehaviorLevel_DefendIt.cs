using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonBehaviorLevel_DefendIt : WorldButton
{
    public LayerMask m_DragLayers;
    [Range (0.0f, 100.0f)]
    public float m_Damping = 1.0f;

    [Range (0.0f, 100.0f)]
    public float m_Frequency = 5.0f;
    
    private TargetJoint2D m_TargetJoint;
    
    private bool isFirstHover = false;
    private Vector3 buttonStartPos;

    protected override void CustomStartBehavior()
    {
        buttonStartPos = transform.position;
        needsClickAnimation = false;
        needsClickSound = false;
        GameManager.Instance.BeginGame();
    }

    protected override void CustomHoverBehavior()
    {
        if (!GameManager.Instance.gameEnds)
        {
            if (!isFirstHover)
            {
                isFirstHover = true;
                _rigidbody.isKinematic = false;
            }
        }
    }

    protected override void CustomHoldingBehavior()
    {
        var worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            var collider = Physics2D.OverlapPoint(worldPos, m_DragLayers);
            if (collider != _collider)
                return;

            // Add a target joint to the Rigidbody2D GameObject.
            m_TargetJoint = _rigidbody.gameObject.AddComponent<TargetJoint2D> ();
            m_TargetJoint.dampingRatio = m_Damping;
            m_TargetJoint.frequency = m_Frequency;
            
            m_TargetJoint.anchor = m_TargetJoint.transform.InverseTransformPoint (worldPos);
        }
        else if (Input.GetMouseButtonUp (0))
        {
            // body.velocity = Vector2.zero;

            Destroy (m_TargetJoint);
            m_TargetJoint = null;
            return;
        }

        if (m_TargetJoint)
        {
            m_TargetJoint.target = worldPos;
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
