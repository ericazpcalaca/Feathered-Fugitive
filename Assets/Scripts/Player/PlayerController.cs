using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

namespace FeatheredFugitive
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _cameraRotationSpeed = 1f;
        [SerializeField] private float _minRotDelta = 0.1f;
        [SerializeField] private float _minPitch = -30f;
        [SerializeField] private float _maxPitch = 60f;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _jumpForce = 4f;
        [SerializeField] private bool _debugEnabled;

        private PlayerInput _playerInput;
        private Rigidbody _rigidbody;
        private Vector2 _moveInput;
        private float _currentCameraYaw;
        private float _currentCameraPitch;
        private uint _playerTokenScore;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _rigidbody = GetComponent<Rigidbody>();
            _playerTokenScore = 0;

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

            //if (_debugEnabled)
            //    Debug.Log($"Current pitch {_currentCameraPitch} Current yaw: {_currentCameraYaw}");
        }

        private void OnPlayerMove(Vector2 vector)
        {
            _moveInput = vector;
        }

        private void MovePlayer()
        {
            // Get the camera's forward and right directions
            Vector3 cameraForward = _cameraTransform.forward;
            Vector3 cameraRight = _cameraTransform.right;

            // Flatten the vectors on the XZ plane to avoid tilting the movement
            cameraForward.y = 0;
            cameraRight.y = 0;

            cameraForward.Normalize();
            cameraRight.Normalize();

            // Convert the input to world space using the camera's orientation
            Vector3 moveDirection = (cameraForward * _moveInput.y + cameraRight * _moveInput.x);

            // Optional: Multiply by a factor to increase speed if needed
            float speedMultiplier = 1.0f; // Adjust this value to your preference

            // Move the player
            Vector3 move = moveDirection * _speed * speedMultiplier * Time.deltaTime;
            _rigidbody.MovePosition(transform.position + move);
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

        private void OnPlayerJump(float value)
        {
            if (value > 0) // Check if the jump key is pressed
            {
                _rigidbody.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.VelocityChange);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {   
            /* 
            * Return to the pool the coin when the player get in contact with it
            */
            if (other.transform.tag == "Token")
            {
                _playerTokenScore++;
                GameObject tokenGameObject = other.gameObject;
                var token = tokenGameObject.GetComponent<Token>();
                TokenManager.Instance.ReturnToPool(token);

                if (_debugEnabled)
                    Debug.Log("Player Score: " + _playerTokenScore);
            }

            /* 
            * In colision with the enemy, check if you were able to detroy it
            */
            if (other.transform.tag == "Enemy")
            {
                GameObject enemyGameObject = other.gameObject;
                var enemy = enemyGameObject.GetComponent<Enemy>();

                // Check if the player collided with the top of the enemy
                float playerHeight = transform.position.y;
                float enemyTopHeight = other.bounds.max.y; 

                if (playerHeight > enemyTopHeight)
                {
                    // Player collided with the top of the enemy
                    Destroy(enemyGameObject);
                }
                else
                {
                    // Player did not collide with the top of the enemy
                    if (_debugEnabled)
                        Debug.Log("Less one life");
                }
            }
        }

    }
}
