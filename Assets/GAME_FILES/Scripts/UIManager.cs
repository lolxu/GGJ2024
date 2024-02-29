using System;
using UnityEngine;

namespace GAME_FILES.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}