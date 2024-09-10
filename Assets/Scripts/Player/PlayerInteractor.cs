using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FeatheredFugitive
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private Transform _interactionPoint;
        [SerializeField] private float _interactionPointRadius = 0.5f;
        [SerializeField] private LayerMask _interactableMask;
        [SerializeField] private GameObject _interactionScreen;
        [SerializeField] private TextMeshProUGUI _interactionText;

        private readonly Collider[] _colliders = new Collider[3];
        [SerializeField] private int _numFound;

        void Update()
        {
            _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

            if (_numFound > 0)
            {
                var interactable = _colliders[0].GetComponent<IInteractable>();
                _interactionScreen.SetActive(true);
                _interactionText.text = interactable.InteractionPrompt;

                if (interactable != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    interactable.Interact(this);                    
                }
            }
            else
            {
                _interactionScreen.SetActive(false);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_interactionPoint.position, _interactionPointRadius);
        }
    }
}