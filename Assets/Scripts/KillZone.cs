using UnityEngine;

namespace DefaultNamespace
{
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MainCamera"))
            {
                GameManager.Instance.RestartGame();
            }
        }
    }
}