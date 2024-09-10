using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatheredFugitive
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _prompt;
        public string InteractionPrompt => _prompt;

        public bool Interact(PlayerInteractor interactor)
        {
            if (InventoryManager.Instance.CheckItem("Chave1"))
            {
                Debug.Log("Opening door");
                InventoryManager.Instance.RemoveItem("Chave1",1);
                return true;
            }

            Debug.Log("Door locked");
            return false;
        }
    }
}