using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatheredFugitive
{
    public class GameStateManager : Singleton<GameStateManager>
    {
        public Action<bool> IsOpenInventory;
        public Action<bool> IsGamePaused;
        public bool HasInventoryOpen { get; private set; }
        public bool HasGamePaused { get; private set; }

        public void GamePaused(bool isPaused)
        {
            IsGamePaused?.Invoke(isPaused);
            HasInventoryOpen = isPaused;
        }

        public void OpenInventory(bool isOpen)
        {
            IsOpenInventory?.Invoke(isOpen);
            HasInventoryOpen = isOpen;
            InventoryManager.Instance.DisplayInventoryOnUI();
        }
    }
}