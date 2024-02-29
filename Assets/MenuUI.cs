using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public TextMeshProUGUI myText;

    private void Start()
    {
        int levelsCompleted = LevelManager.Instance.CountCompletedLevels();
        myText.text = "V " + levelsCompleted + ".6";
    }
}
