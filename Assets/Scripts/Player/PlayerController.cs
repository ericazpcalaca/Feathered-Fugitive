using Cinemachine;
using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

namespace FeatheredFugitive
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private float _gravityValue = -9.81f;
        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField] private float _playerSpeed = 2f;
        [SerializeField] private bool _debugEnabled;

        [SerializeField] private CinemachineFreeLook _cinemachineCamera;
        [SerializeField] private float _cameraDisableDuration = 0.3f; 


        private CharacterController _controller;
        private Transform _cameraMainTransform;
        private PlayerInput _playerInput;
        private Vector3 _playerVelocity;
        private Vector2 _moveInput;
        private bool _groundedPlayer;

        private uint _playerTokenScore;
        private uint _playerLife;
        public Action<uint> UpdateScore;
        public Action<uint> UpdateLife;

        private void Awake()
        {
            _controller = gameObject.GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();
            _cameraMainTransform = Camera.main.transform;
            

            SetUpPlayer();

            _playerInput.OnPlayerMove += OnPlayerMove;
            _playerInput.OnPlayerJump += OnPlayerJump;

            GameStateManager.Instance.IsOpenInventory += HandleInventory;
            GameStateManager.Instance.IsGamePaused += HandleGamePaused;
        }

        private void OnDestroy()
        {
            _playerInput.OnPlayerMove -= OnPlayerMove;
            _playerInput.OnPlayerJump -= OnPlayerJump;

            GameStateManager.Instance.IsOpenInventory -= HandleInventory;
            GameStateManager.Instance.IsGamePaused -= HandleGamePaused;
        }

        private void Update()
        {
            MovePlayer();
        }

        private void OnTriggerEnter(Collider other)
        {
            /* 
            * Return to the pool the coin when the player get in contact with it
            */
            if (other.transform.tag == "Token")
            {
                _playerTokenScore++;
                UpdateScore?.Invoke(_playerTokenScore);

                GameObject tokenGameObject = other.gameObject;
                var token = tokenGameObject.GetComponent<Token>();
                
                TokenManager.Instance.ReturnToPool(token);
                InventoryManager.Instance.AddItem("Moedinha", 1);

                if (_debugEnabled)
                {
                    InventoryManager.Instance.DisplayInventory();
                    Debug.Log("Player Score: " + _playerTokenScore);
                }
            }

            /* 
            * In colision with the enemy, check if you were able to detroy it
            */
            if (other.CompareTag("Enemy"))
            {
                GameObject enemyGameObject = other.gameObject;
                float enemyTopHeight = other.bounds.max.y;

                RaycastHit hit;
                float playerFeetHeight = transform.position.y;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
                {
                    playerFeetHeight = hit.point.y;  // Get the Y position where the ray hits
                }

                if (_debugEnabled)
                {
                    Debug.Log("Player Feet Height: " + playerFeetHeight);
                    Debug.Log("Enemy Top Height: " + enemyTopHeight);
                }

                if (playerFeetHeight > enemyTopHeight)
                {
                    Destroy(enemyGameObject);
                    if (_debugEnabled)
                        Debug.Log("Destroyed");
                }
                else
                {
                    //_playerLife--;
                    //UpdateLife?.Invoke(_playerLife);
                    if (_debugEnabled)
                        Debug.Log("Less one life");
                }
            }
        }

        private void SetUpPlayer()
        {
            _playerTokenScore = 0;
            _playerLife = 3;
            UpdateScore?.Invoke(_playerTokenScore);
            UpdateLife?.Invoke(_playerLife);
        }

        private void OnPlayerMove(Vector2 vector)
        {
            _moveInput = vector;
        }

        private void OnPlayerJump(float value)
        {
            if (value > 0 && _groundedPlayer)
            {
                _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);
                
                if (_debugEnabled)
                    Debug.Log("Player Height jump: " + transform.position.y);
            }
        }

        private void MovePlayer()
        {
            _groundedPlayer = _controller.isGrounded;
            if (_groundedPlayer && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }

            Vector3 moveNewDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
            moveNewDirection = _cameraMainTransform.forward * moveNewDirection.z + _cameraMainTransform.right * moveNewDirection.x;
            moveNewDirection.y = 0f;

            _controller.Move(moveNewDirection * Time.deltaTime * _playerSpeed);

            if (_moveInput != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(_moveInput.x, _moveInput.y) * Mathf.Rad2Deg + _cameraMainTransform.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
            }

            // Apply gravity
            float fallMultiplier = 2.0f;
            float newYVelocity = _playerVelocity.y + (_gravityValue * fallMultiplier * Time.deltaTime);
            _playerVelocity.y = newYVelocity;
            _controller.Move(_playerVelocity * Time.deltaTime);
        }

        private void HandleInventory(bool isOpen)
        {
            if (!isOpen)
            {
                _playerInput.ResumeGame();
            }
        }

        private void HandleGamePaused(bool isPaused)
        {
            if (!isPaused)
            {
                _playerInput.ResumeGame();
            }
        }

        public void Teleport(Vector3 position, Quaternion rotation)
        {
            _cinemachineCamera.enabled = false;

            transform.position = position;
            transform.rotation = rotation;
            Physics.SyncTransforms();
            _playerVelocity = Vector3.zero;

            StartCoroutine(ReEnableCameraAfterTeleport());
        }

        private IEnumerator ReEnableCameraAfterTeleport()
        {
            yield return new WaitForSeconds(_cameraDisableDuration);
            _cinemachineCamera.enabled = true;
        }
    }
}
