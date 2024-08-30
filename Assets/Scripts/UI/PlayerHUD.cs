using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FeatheredFugitive
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private GameObject _inventoryScreen;

        private PlayerInput _playerInput;

        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            _inventoryScreen.SetActive(false);

            _playerInput.OpenInventory += HandleInventory;
        }

        private void OnDestroy()
        {
            _playerInput.OpenInventory -= HandleInventory;
        }
       
        private void HandleInventory()
        {
            _inventoryScreen.SetActive(true);
        }
     
    }
}