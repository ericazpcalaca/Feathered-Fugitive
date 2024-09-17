using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatheredFugitive
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] Transform destination;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.TryGetComponent<PlayerController>(out var player))
            {
                player.Teleport(destination.position, destination.rotation);
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(destination.position, .4f);
            var direction = destination.TransformDirection(Vector3.forward);
            Gizmos.DrawRay(destination.position, direction);
        }
    }
}