using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GAME_FILES.Scripts.PlayButton;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GAME_FILES.Scripts
{
    public class Enemy_DefendIt : MonoBehaviour
    {
        public Rigidbody2D _rigidbody;
        private Vector3 targetPosition = Vector2.zero;
        private float moveSpeed = 1.0f;
        
        private void Start()
        {
            targetPosition = GameObject.FindGameObjectWithTag("QuitButton").transform.position;
            moveSpeed = Random.Range(2.0f, 7.5f);
        }

        private void FixedUpdate()
        {
            Vector2 moveDirection = targetPosition - transform.position;
            
            if (targetPosition != Vector3.zero)
            {
                _rigidbody.MovePosition(_rigidbody.position + moveDirection.normalized * moveSpeed * Time.deltaTime);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (gameObject)
            {
                if (other.collider.CompareTag("StartButton"))
                {
                    EnemySpawner.Instance.EliminatedEnemy(gameObject);
                    FeelMaster.Instance.PlayEnemyDeath(gameObject.transform.position, 0.4f);
                    Destroy(gameObject);
                }
                else if (other.collider.CompareTag("QuitButton"))
                {
                    EnemySpawner.Instance.EliminatedEnemy(gameObject);
                    FeelMaster.Instance.PlayEnemyDeath(gameObject.transform.position, 2.5f);
                    // GameManager.Instance.DecreaseLife(1);
                    var script = other.gameObject.GetComponent<QuitButtonBehavior>();
                    script.StartQuit();
                    Destroy(gameObject);
                }
            }
        }
        
        public void ForceKill()
        {
            FeelMaster.Instance.PlayEnemyDeathParticles(gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}