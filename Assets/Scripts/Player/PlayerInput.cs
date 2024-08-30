using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static FeatheredFugitive.Input;

namespace FeatheredFugitive
{
    public class PlayerInput : MonoBehaviour, IPlayerActions

    {
        public Action<Vector2> OnPlayerMoveCamera;
        public Action<Vector2> OnPlayerMove;
        public Action<float> OnPlayerJump;
        public Action OpenInventory;

        private Input _input;
        private bool _isPaused;

        private void Awake()
        {
            _input = new();
            _input.Player.AddCallbacks(this);
            _input.Player.Enable();
            _isPaused = false;
            ShowMouse(false);
        }

        private void OnDestroy()
        {
            _input?.Player.RemoveCallbacks(this);
            _input?.Player.Disable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            OnPlayerJump?.Invoke(context.ReadValue<float>());
        }

        public void OnMoveCamera(InputAction.CallbackContext context)
        {
            if (!context.performed || _isPaused)
                return;

            OnPlayerMoveCamera?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnMovePlayer(InputAction.CallbackContext context)
        {
            OnPlayerMove?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnOpenInventory(InputAction.CallbackContext context)
        {
            _isPaused = true;
            OpenInventory?.Invoke();
            ShowMouse(true);
        }

        public void ShowMouse(bool show)
        {
            Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = show;
        }

        public void ResumeGame()
        {
            _isPaused = false;
        }
    }
}