using UnityEngine;

namespace DefaultNamespace
{
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.RestartGame();
            }
        }
    }
}