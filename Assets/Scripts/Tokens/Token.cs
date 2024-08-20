using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatheredFugitive
{
    public class Token : MonoBehaviour
    {
        [SerializeField] private uint _targetScore;        
        private Vector3 rotationSpeed = new Vector3(0, 0, 100);
  
        public uint TargetScore
        {
            get { return _targetScore; }
            set { _targetScore = value; }
        }

        private void Update()
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }       

    }
}