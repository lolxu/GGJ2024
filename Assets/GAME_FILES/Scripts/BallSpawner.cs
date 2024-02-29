using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;

    public static BallSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnBall()
    {
        if (!GameManager.Instance.gameEnds)
        {
            Debug.Log("Spawned Ball");
            GameObject b = Instantiate(ball, transform.position, Quaternion.identity);
        }
    }
}
