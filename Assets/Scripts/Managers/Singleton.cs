using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatheredFugitive
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if ((object)_instance == null)
                {
                    _instance = (T)FindAnyObjectByType(typeof(T));
                    if (_instance == null)
                    {
                        FindOrCreateNewInstance();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            SetInstanceAndRemoveDuplicates();
        }

        private static void FindOrCreateNewInstance()
        {
            // Try to find existing instance
            _instance = (T)FindAnyObjectByType(typeof(T));

            if (_instance == null)
            {
                // Since the instance was not found, create new one
                GameObject gameObject = new GameObject();
                gameObject.name = typeof(T).Name;
                _instance = gameObject.AddComponent<T>();
            }
        }

        private void SetInstanceAndRemoveDuplicates()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}