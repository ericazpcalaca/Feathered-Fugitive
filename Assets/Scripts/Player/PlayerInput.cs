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
        public Action<Vector2> OnPlayerJump;

        private Input _input;

        private void Awake()
        {
            _input = new();
            _input.Player.AddCallbacks(this);
            _input.Player.Enable();
        }

        private void OnDestroy()
        {
            _input?.Player.RemoveCallbacks(this);
            _input?.Player.Disable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            OnPlayerJump?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnMoveCamera(InputAction.CallbackContext context)
        {
            OnPlayerMoveCamera?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnMovePlayer(InputAction.CallbackContext context)
        {
            OnPlayerMove?.Invoke(context.ReadValue<Vector2>());
        }
        
    }
}