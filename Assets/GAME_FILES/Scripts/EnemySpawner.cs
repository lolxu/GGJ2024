using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GAME_FILES.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance;

        public GameObject enemyPrefab;
        public List<GameObject> enemies;
        public int enemySpawnCount = 10;
        public float spawnRadius = 10.0f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            // TODO make enemies spawn with game manager

            GameManager.Instance.OnPlayerLoses += CleanEnemiesOnScreen;
            GameManager.Instance.OnPlayerWins += CleanEnemiesOnScreen;
        }

        public void SpawnEnemies()
        {
            for (int i = 0; i < enemySpawnCount; i++)
            {
                var randomDirection = Random.insideUnitCircle.normalized;
                Collider2D collider = Physics2D.OverlapCircle(randomDirection * spawnRadius, 5.0f);
                while (collider)
                {
                    randomDirection = Random.insideUnitCircle.normalized;
                    collider = Physics2D.OverlapPoint(randomDirection * spawnRadius);
                }
                GameObject enemy = Instantiate(enemyPrefab, randomDirection * spawnRadius, Quaternion.identity);
                var enemyScript = enemy.GetComponent<Enemy_MoveIt>();
                if (enemyScript)
                {
                    enemyScript.moveDirection = -randomDirection;
                }
                enemies.Add(enemy);
            }
        }

        public void EliminatedEnemy(GameObject enemy)
        {
            if (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
            }
            if (enemies.Count <= Random.Range(0, 5))
            {
                SpawnEnemies();
            }
        }

        public void CleanEnemiesOnScreen()
        {
            foreach (var enemy in enemies)
            {
                Enemy_DefendIt script_defend = enemy.GetComponent<Enemy_DefendIt>();
                if (script_defend)
                {
                    script_defend.ForceKill();
                }
                else
                {
                    Enemy_MoveIt script_move = enemy.GetComponent<Enemy_MoveIt>();
                    script_move.ForceKill();
                }
            }
            enemies.Clear();
        }
    }
}