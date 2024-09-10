using UnityEngine;
using UnityEngine.UI;

namespace FeatheredFugitive
{
    public class PauseHUD : MonoBehaviour
    {
        [SerializeField] private Button _btnCloseScreen;
        [SerializeField] private Button _btnReturnGame;
        [SerializeField] private Button _btnQuit;
        [SerializeField] private GameObject _pauseScreen;

        void Start()
        {
            _btnCloseScreen.onClick.AddListener(OnExitClick);
            _btnReturnGame.onClick.AddListener(OnExitClick);
            _btnQuit.onClick.AddListener(OnQuitGame);
        }

        private void OnQuitGame()
        {
            Debug.Log("Go to initial menu");
        }

        private void OnExitClick()
        {
            _pauseScreen.SetActive(false);
            GameStateManager.Instance.IsOpenInventory(false);
        }
    }
}