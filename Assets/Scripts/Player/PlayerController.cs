using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace FeatheredFugitive
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _cameraRotationSpeed = 1f;
        [SerializeField] private float _minRotDelta = 0.1f;
        [SerializeField] private float _minPitch = -30f;
        [SerializeField] private float _maxPitch = 60f;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _jumpForce = 15f;
        [SerializeField] private bool _debugEnabled;

        private PlayerInput _playerInput;
        private float _currentCameraYaw;
        private float _currentCameraPitch;
        private Vector2 _moveInput;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();

            _playerInput.OnPlayerMoveCamera += OnPlayerMoveCamera;
            _playerInput.OnPlayerMove += OnPlayerMove;
            _playerInput.OnPlayerJump += OnPlayerJump;
        }

        private void OnDestroy()
        {
            _playerInput.OnPlayerMoveCamera -= OnPlayerMoveCamera;
            _playerInput.OnPlayerMove -= OnPlayerMove;
            _playerInput.OnPlayerJump -= OnPlayerJump;
        }

        private void Update()
        {
            MovePlayer();
            RotatePlayerWithCamera();
            FollowPlayerWithCamera();
        }

        private void OnPlayerMoveCamera(Vector2 posDelta)
        {
            if (Mathf.Abs(posDelta.x) > _minRotDelta)
            {
                _currentCameraYaw += posDelta.x * _cameraRotationSpeed * Time.deltaTime;
            }

            if (Mathf.Abs(posDelta.y) > _minRotDelta)
            {
                _currentCameraPitch += (posDelta.y * -1) * _cameraRotationSpeed * Time.deltaTime;
                _currentCameraPitch = Mathf.Clamp(_currentCameraPitch, _minPitch, _maxPitch);
            }

            if (_debugEnabled)
                Debug.Log($"Current pitch {_currentCameraPitch} Current yaw: {_currentCameraYaw}");
        }

        private void OnPlayerMove(Vector2 vector)
        {
            _moveInput = vector;
        }

        private void MovePlayer()
        {
            Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y) * _speed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }

        /*
         * Rotate the player to face the direction of movement
         */
        private void RotatePlayerWithCamera()
        {
            if (_moveInput.sqrMagnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.Euler(0, _currentCameraYaw, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _cameraRotationSpeed * Time.deltaTime);
            }
        }

        /* 
         * Position the camera at a distance from the player and rotate around them
         * It was adjusted as needed and in the end, was adjust to look at the player's upper body/head
         */
        private void FollowPlayerWithCamera()
        {            
            Vector3 offset = new Vector3(0, 2f, -5f); 
            Quaternion rotation = Quaternion.Euler(_currentCameraPitch, _currentCameraYaw, 0);
            _cameraTransform.position = transform.position + rotation * offset;
            _cameraTransform.LookAt(transform.position + Vector3.up * 1.5f);  
        }

        private void OnPlayerJump(Vector2 vector)
        {
            Vector3 move = new Vector3(_moveInput.x, 15, _moveInput.y) * _jumpForce * Time.deltaTime;
            transform.Translate(move, Space.World);
        }

    }
}
