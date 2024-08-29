using System;
using System.Collections;
using UnityEngine;
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

        private CharacterController _controller;
        private Transform _cameraMainTransform;
        private PlayerInput _playerInput;
        private Vector3 _playerVelocity;
        private Vector2 _moveInput;
        private bool _groundedPlayer;
        private uint _playerTokenScore;

        private void Awake()
        {
            _controller = gameObject.GetComponent<CharacterController>();
            _cameraMainTransform = Camera.main.transform;
            _playerInput = GetComponent<PlayerInput>();
            _playerTokenScore = 0;

            _playerInput.OnPlayerMove += OnPlayerMove;
            _playerInput.OnPlayerJump += OnPlayerJump;

        }

        private void OnDestroy()
        {
            _playerInput.OnPlayerMove -= OnPlayerMove;
            _playerInput.OnPlayerJump -= OnPlayerJump;
        }

        void Update()
        {
            MovePlayer();
            float playerHeight = transform.position.y;
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

                if (_debugEnabled)
                {
                    Debug.Log("Player Height: " + playerHeight);
                    Debug.Log("Enemy Top Height: " + enemyTopHeight);
                }


                if (playerHeight > enemyTopHeight )
                {
                    // Player collided with the top of the enemy
                    Destroy(enemyGameObject);
                    Debug.Log("entro");
                }
                else
                {
                    // Player did not collide with the top of the enemy
                    if (_debugEnabled)
                        Debug.Log("Less one life");
                }
            }
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

    }
}
