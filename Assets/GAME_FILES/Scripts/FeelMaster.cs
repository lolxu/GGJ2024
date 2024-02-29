using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class FeelMaster : MonoBehaviour
{
    public MMF_Player CamShake;
    public MMF_Player EnemyDeathParticles;
    public MMF_Player HitStop;
    public MMF_Player PlayerHitParticles;
    public MMF_Player PongHitParticles;
    public MMF_Player EnemyOnlyDeathParticles;

    public static FeelMaster Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayCamShake(float amplitude)
    {
        CamShake.PlayFeedbacks(transform.position, amplitude);
    }

    public void PlayEnemyDeath(Vector3 position, float amplitude)
    {
        CamShake.PlayFeedbacks(transform.position, amplitude);
        EnemyDeathParticles.PlayFeedbacks(position);
    }

    public void PlayEnemyDeathParticles(Vector3 position)
    {
        EnemyDeathParticles.PlayFeedbacks(position);
    }

    public void PlayEnemyDeathParticlesOnly(Vector3 position)
    {
        EnemyOnlyDeathParticles.PlayFeedbacks(position);
    }

    public void PlayPlayerHit(Vector3 position, float amplitude)
    {
        CamShake.PlayFeedbacks(transform.position, amplitude);
        PlayerHitParticles.PlayFeedbacks(position);
        HitStop.PlayFeedbacks();
    }

    public void PlayPongHit(Vector3 position, float amplitude)
    {
        PongHitParticles.PlayFeedbacks(position, amplitude);
    }
}
