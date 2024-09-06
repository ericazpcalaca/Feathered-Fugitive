using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace FeatheredFugitive
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _lifeText;
        [SerializeField] private GameObject _inventoryScreen;
        [SerializeField] private GameObject _pauseScreen;

        PlayerController _playerController;

        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            _playerController.UpdateScore += OnScoreUpdated;
            _playerController.UpdateLife += OnLifeUpdated;


            _inventoryScreen.SetActive(false);
            _pauseScreen.SetActive(false);
            GameStateManager.Instance.IsOpenInventory += HandleInventory;
            GameStateManager.Instance.IsGamePaused += HandleGamePaused;
        }        

        private void OnDestroy()
        {
            _playerController.UpdateScore -= OnScoreUpdated;
            _playerController.UpdateLife -= OnLifeUpdated;

            GameStateManager.Instance.IsOpenInventory -= HandleInventory;
            GameStateManager.Instance.IsGamePaused += HandleGamePaused;
        }

        private void HandleInventory(bool isOpen)
        {
            _inventoryScreen.SetActive(isOpen);
        }

        private void HandleGamePaused(bool isPaused)
        {
            _pauseScreen.SetActive(isPaused);
        }

        private void OnScoreUpdated(uint newScore)
        {
            _scoreText.text = $"{newScore.ToString("D6")}";
        }

        private void OnLifeUpdated(uint updateLife)
        {
            _lifeText.text = $"{updateLife}";
        }
    }
}