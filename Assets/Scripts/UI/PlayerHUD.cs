using TMPro;
using UnityEngine;

namespace FeatheredFugitive
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private GameObject _inventoryScreen;
        [SerializeField] private GameObject _pauseScreen;

        private void Start()
        {
            _inventoryScreen.SetActive(false);
            _pauseScreen.SetActive(false);

            GameStateManager.Instance.IsOpenInventory += HandleInventory;
            GameStateManager.Instance.IsGamePaused += HandleGamePaused;
        }

        private void OnDestroy()
        {
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
    }
}