using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatheredFugitive
{
    public interface IInteractable
    {
        public string InteractionPrompt { get; }
        public bool Interact (PlayerInteractor interactor);
    }
}