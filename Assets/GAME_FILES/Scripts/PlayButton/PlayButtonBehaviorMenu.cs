using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonBehaviorMenu : WorldButton
{
    private bool isFirstHover = false;
    private Vector3 buttonStartPos; 
    protected override void CustomStartBehavior()
    {
        buttonStartPos = transform.position;
    }

    protected override void CustomClickBehavior()
    {
        Debug.Log("Menu Play Click");
        string levelName = LevelManager.Instance.ChooseARandomUncompletedLevel();
        if (String.IsNullOrEmpty(levelName))
        {
            Debug.Log("ALL DONE");
        }
        else
        {
            LevelManager.Instance.LoadLevel(levelName);
        }
    }
}
