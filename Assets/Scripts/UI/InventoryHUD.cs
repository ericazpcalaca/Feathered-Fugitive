using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FeatheredFugitive
{
    public class InventoryHUD : MonoBehaviour
    {
        [SerializeField] private Button _btnCloseInventory;
        [SerializeField] private GameObject _inventoryScreen;

        void Start()
        {
            _btnCloseInventory.onClick.AddListener(OnExitClick);
        }

        private void OnExitClick()
        {
            _inventoryScreen.SetActive(false);
        }
       
    }
}