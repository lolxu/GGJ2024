using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private ModalWindow _modalWindow;

    public ModalWindow modalWindow => _modalWindow;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
