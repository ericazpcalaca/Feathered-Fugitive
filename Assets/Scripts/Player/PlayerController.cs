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
        [SerializeField] private Transform _cameraLookAt;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _cameraRotationSpeed = 1;
        [SerializeField] private float _minRotDelta = 10f;
        [SerializeField] private float _minPitch = -30f;
        [SerializeField] private float _maxPitch = 60f;
        [SerializeField] private float _speed = 2f;
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
            CalculateInitialCameraRotation();
        }

        private void OnDestroy()
        {
            _playerInput.OnPlayerMoveCamera -= OnPlayerMoveCamera;
            _playerInput.OnPlayerMove -= OnPlayerMove;
        }

        private void Update()
        {
            MovePlayer();
            RotatePlayerWithCamera();
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

            // Apply the rotation to the camera
            _cameraTransform.localRotation = Quaternion.Euler(_currentCameraPitch, 0, 0);
        }

        private void CalculateInitialCameraRotation()
        {
            Vector3 lookDirection = (_cameraLookAt.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(lookDirection);
            _currentCameraYaw = transform.eulerAngles.y;
            _currentCameraPitch = transform.eulerAngles.x;

            if (_debugEnabled)
                Debug.Log($"Initial camera rotation set. Yaw: {_currentCameraYaw}, Pitch: {_currentCameraPitch}");
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

        private void RotatePlayerWithCamera()
        {
            // Rotate the player based on the camera's yaw rotation
            transform.rotation = Quaternion.Euler(0, _currentCameraYaw, 0);
        }
    }
}
