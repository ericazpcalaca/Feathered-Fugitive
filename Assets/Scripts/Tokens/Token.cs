using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatheredFugitive
{
    public class Token : MonoBehaviour
    {
        [SerializeField] private uint _targetScore;
        [SerializeField] private string _boundaryLayerName;
        
        private Quaternion _originalRotation;
        private Vector3 rotationSpeed = new Vector3(0, 0, 100);

        public uint TargetScore
        {
            get { return _targetScore; }
            set { _targetScore = value; }
        }

        private int _boundaryLayer;
        private int _currentHit;

        private void Awake()
        {
            _boundaryLayer = LayerMask.NameToLayer(_boundaryLayerName);
            _originalRotation = transform.rotation;
        }

        private void OnDestroy()
        {

        }

        private void Update()
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            //if (collision.gameObject.layer == _boundaryLayer)
            //{
            //    TargetManager.Instance.ReturnToPool(this);
            //}
        }
    
    }
}