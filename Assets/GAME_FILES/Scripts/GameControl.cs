using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameControl : MonoBehaviour
{
    public enum ControlStyles
    {
        Clicks,
        Move,
        Idle
    };
    
    [Header("Collision LayerMask settings")]
    public LayerMask worldButtonMask;

    [Header("Control Style Settings")] 
    public ControlStyles levelControlStyle;
    public float buttonMoveSpeed = 10.0f;
    public bool isButtonKinematic = false;
    public float buttonGravity = 1.0f;
    
    [Header("Audio Settings")]
    [SerializeField] private List<AudioClip> clickSounds;

    public static GameControl Instance;
    public bool isHoveringStartButton { private set; get; } = false;
    public bool isClickingStartButton { private set; get; } = false;
    public bool isMovingStartButton { private set; get; } = false;

    public bool isHoveringQuitButton { private set; get; } = false;
    public bool isClickingQuitButton { private set; get; } = false;

    private bool isMouseClickedWhenNotHovering = false;
    
    private GameObject startButton;
    private Rigidbody2D startButtonRigidBody;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        startButton = GameObject.FindGameObjectWithTag("StartButton");

        if (startButton)
        {
            startButtonRigidBody = startButton.GetComponent<Rigidbody2D>();

            startButtonRigidBody.isKinematic = isButtonKinematic;
            startButtonRigidBody.gravityScale = buttonGravity;
        }
    }

    private void Update()
    {
        if (levelControlStyle == ControlStyles.Clicks)
        {
            MouseControl();
        }
        else if (levelControlStyle == ControlStyles.Move)
        {
            MoveControl();
        }
    }

    private void MouseControl()
    {
        Vector3 mouseRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var collider = Physics2D.OverlapPoint(mouseRay, worldButtonMask);

        if (!collider)
        {
            isHoveringStartButton = false;
            isClickingStartButton = false;
            isHoveringQuitButton = false;
            isClickingQuitButton = false;
            isMouseClickedWhenNotHovering = Input.GetMouseButton(0);
            return;
        }
        if (collider.CompareTag("StartButton") && !isMouseClickedWhenNotHovering)
        {
            isHoveringStartButton = true;
            isClickingStartButton = Input.GetMouseButton(0);
        }
        else if (collider.CompareTag("QuitButton") && !isMouseClickedWhenNotHovering)
        {
            isHoveringQuitButton = true;
            isClickingQuitButton = Input.GetMouseButton(0);
        }
        else
        {
            isClickingStartButton = false;
            isClickingQuitButton = false;
        }
    }

    public void PlayClickSound()
    {
        if (LevelManager.Instance)
        {
            LevelManager.Instance.audioSource.pitch = Random.Range(0.75f, 1.0f);
            LevelManager.Instance.audioSource.PlayOneShot(clickSounds[Random.Range(0, clickSounds.Count)]);
        }
    }

    private void MoveControl()
    {
        if (!GameManager.Instance.gameEnds)
        {
            var moveX = Input.GetAxis("Horizontal");
            var moveY = Input.GetAxis("Vertical");

            var moveDirection = new Vector3(moveX, moveY, 0.0f);

            if (moveDirection.normalized != Vector3.zero)
            {
                isMovingStartButton = true;
            }
            else
            {
                isMovingStartButton = false;
            }
            
            // Debug.Log(isMovingStartButton);
            
            startButtonRigidBody.MovePosition(startButton.transform.position + moveDirection * buttonMoveSpeed);
        }
    }
}
