using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatheredFugitive
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _prompt;
        public string InteractionPrompt => _prompt;

        public bool Interact(PlayerInteractor interactor)
        {
            Debug.Log("Opening chest");
            return true;
        }
    }
}