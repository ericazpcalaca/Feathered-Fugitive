using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatheredFugitive
{
    public class Enemy : MonoBehaviour
    {
        public enum MovementType
        {
            Left,
            Right,
            FollowPlayer
        }

        [SerializeField] private MovementType _movementType;
        [SerializeField] private float _speed = 2f;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public MovementType CurrentMovementType
        {
            get { return _movementType; }
            set { _movementType = value; }
        }

        private void Update()
        {            
            switch (_movementType)
            {
                case MovementType.Left:
                    MoveLeft();
                    break;
                case MovementType.Right:
                    MoveRight();
                    break;
                case MovementType.FollowPlayer:
                    FollowPlayer();
                    break;
            }
        }

        private void MoveLeft()
        {
            transform.position += transform.right * _speed * -1 * Time.deltaTime;
        }

        private void MoveRight()
        {
            transform.position += transform.right * _speed * Time.deltaTime;
        }

        private void FollowPlayer()
        {
            Debug.Log("Create the follow player latter");
        }

    }
}
