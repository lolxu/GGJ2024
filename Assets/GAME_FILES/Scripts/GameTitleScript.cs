using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTitleScript : MonoBehaviour
{
    private TextMeshProUGUI myText;
    private TextMeshPro myTextTitle;
    
    private void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
        myTextTitle = GetComponent<TextMeshPro>();
    }

    public void ChangeText(string newText)
    {
        if (myText)
        {
            myText.text = newText;
        }
        else if (myTextTitle)
        {
            myTextTitle.text = newText;
        }
    }
}
