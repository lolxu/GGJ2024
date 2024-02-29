using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeroWindowTrigger : MonoBehaviour
{
    public string title;
    public Sprite sprite;
    public string message;
    public bool triggerOnEnable;

    public UnityEvent onContinueEvent;
    public UnityEvent onCancelEvent;
    public UnityEvent onAlternateEvent;
    
    private void Start()
    {
        if (!triggerOnEnable)
        {
            return;
        }

        Action continueCallback = null;
        Action cancelCallback = null;
        Action alternateCallback = null;

        if (onContinueEvent.GetPersistentEventCount() > 0)
        {
            continueCallback = onContinueEvent.Invoke;
        }
        
        if (onCancelEvent.GetPersistentEventCount() > 0)
        {
            cancelCallback = onCancelEvent.Invoke;
        }
        
        if (onAlternateEvent.GetPersistentEventCount() > 0)
        {
            alternateCallback = onAlternateEvent.Invoke;
        }
        
        // Debug.Log(UIController.Instance);
        UIController.Instance.modalWindow.ShowAsHero(title, sprite, message, continueCallback, cancelCallback, alternateCallback);
    }
}
