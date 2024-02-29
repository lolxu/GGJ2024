using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GAME_FILES.Scripts
{
    public class Enemy_MoveIt : MonoBehaviour
    {
        public Rigidbody2D _rigidbody;
        public Vector2 moveDirection;
        private float moveForce = 1.0f;
        
        private void Start()
        {
            moveForce = Random.Range(5.0f, 12.5f);
            _rigidbody.AddForce(moveDirection.normalized * moveForce, ForceMode2D.Impulse);
            StartCoroutine(StartDecay());
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("StartButton"))
            {
                EnemySpawner.Instance.EliminatedEnemy(gameObject);
                FeelMaster.Instance.PlayPlayerHit(gameObject.transform.position, 1.5f);
                GameManager.Instance.DecreaseLife(1);
                Destroy(gameObject);
            }
        }

        IEnumerator StartDecay()
        {
            yield return new WaitForSeconds(5.0f);
            EnemySpawner.Instance.EliminatedEnemy(gameObject);
            FeelMaster.Instance.PlayEnemyDeathParticlesOnly(gameObject.transform.position);
            Destroy(gameObject);
        }

        public void ForceKill()
        {
            FeelMaster.Instance.PlayEnemyDeathParticles(gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}