using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace FeatheredFugitive
{
    public class TokenManager : Singleton<TokenManager>
    {
        [SerializeField] private uint _initPoolSize;
        [SerializeField] private GameObject _objectToPool;

        private Stack<Token> _tokenPool;

        private void Start()
        {
            SetupPool();
        }

        private void SetupPool()
        {
            _tokenPool = new Stack<Token>();
            for (int i = 0; i < _initPoolSize; i++)
            {
                GameObject tokenGameObject = Instantiate(_objectToPool);
                Token token = tokenGameObject.GetComponent<Token>();
                if (tokenGameObject == null)
                {
                    Debug.LogError($"[{typeof(TokenManager).Name}] - Can't instantiate target from prefab!");
                    continue;
                }

                tokenGameObject.SetActive(false);
                _tokenPool.Push(token);
            }
        }

        private Token GetFromPool()
        {
            if (_tokenPool.Count > 0)
            {
                Token token = _tokenPool.Pop();
                token.gameObject.SetActive(true);
                return token;
            }
            else
            {
                GameObject tokenGameObject = Instantiate(_objectToPool);
                return tokenGameObject.GetComponent<Token>();
            }
        }

        public void ReturnToPool(Token token)
        {
            if (!_tokenPool.Contains(token))
            {
                _tokenPool.Push(token);
            }
            token.gameObject.SetActive(false);
        }

    }
}