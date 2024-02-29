using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldButton : MonoBehaviour
{
    public Rigidbody2D _rigidbody;
    public SpriteRenderer _renderer;
    public Collider2D _collider;
    public TextMeshPro _myText;

    public Color hoverColor;
    public Color clickedColor;
    private Color orgColor;
    protected bool isHovering = false;
    protected bool isClicked = false;
    private Vector3 orgScale;

    protected bool needsClickAnimation = true;
    protected bool needsClickSound = true;
    public bool canClick = true;

    protected virtual void CustomStartBehavior() { }
    protected virtual void CustomUpdateBehavior() { }
    
    protected virtual void CustomHoverBehavior() { }
    
    protected virtual void CustomClickBehavior() { }
    
    protected virtual void CustomHoldingBehavior() { }

    protected virtual void WinBehavior() { }
    
    protected virtual void LoseBehavior() { }

    private void Start()
    {
        orgColor = _renderer.color;
        orgScale = transform.localScale;
        GameManager.Instance.OnPlayerWins += WinBehavior;
        GameManager.Instance.OnPlayerLoses += LoseBehavior;
        CustomStartBehavior();
    }

    private void Update()
    {
        CheckHovering();
        CheckClicking();
        CustomUpdateBehavior();
        
        CheckIsHolding();
    }

    private void CheckHovering()
    {
        if (gameObject.CompareTag("StartButton"))
        {
            if (GameControl.Instance.isHoveringStartButton != isHovering)
            {
                isHovering = !isHovering;
                _renderer.color = isHovering ? hoverColor : orgColor;
                // Debug.Log("Hovering changed");
                CustomHoverBehavior();
            }
        }
        else
        {
            if (GameControl.Instance.isHoveringQuitButton != isHovering)
            {
                isHovering = !isHovering;
                _renderer.color = isHovering ? hoverColor : orgColor;
                // Debug.Log("Hovering changed");
                CustomHoverBehavior();
            }
        }
        
    }

    private void CheckClicking()
    {
        if (gameObject.CompareTag("StartButton"))
        {
            if (GameControl.Instance.isClickingStartButton != isClicked)
            {
                isClicked = !isClicked;

                if (isClicked)
                {
                    // transform.DOKill();
                    transform.localScale = orgScale;
                    if (needsClickAnimation)
                    {
                        transform.DOScale(transform.localScale * 0.85f, 0.05f).SetLoops(2, LoopType.Yoyo)
                            .SetEase(Ease.InOutElastic);
                    }

                    if (needsClickSound)
                    {
                        GameControl.Instance.PlayClickSound();
                    }
                    
                    CustomClickBehavior();
                }
            
                _renderer.color = isClicked ? clickedColor : hoverColor;
                transform.localScale = orgScale;
                // Debug.Log("UnClicked");
            }
        }
        else
        {
            if (GameControl.Instance.isClickingQuitButton != isClicked && canClick)
            {
                isClicked = !isClicked;

                if (isClicked)
                {
                    // transform.DOKill();
                    Debug.Log("Quit Button Clicked");
                    transform.localScale = orgScale;
                    if (needsClickAnimation)
                    {
                        transform.DOScale(transform.localScale * 0.85f, 0.05f).SetLoops(2, LoopType.Yoyo)
                            .SetEase(Ease.InOutElastic);
                    }
                    CustomClickBehavior();
                }
            
                _renderer.color = isClicked ? clickedColor : hoverColor;
                transform.localScale = orgScale;
                // Debug.Log("UnClicked");
            }
        }
        
    }

    private void CheckIsHolding()
    {
        // if (GameControl.Instance.isClickingStartButton)
        // {
        //     Debug.Log("Is Holding");
        //     CustomHoldingBehavior();
        // }
        CustomHoldingBehavior();
    }
}
