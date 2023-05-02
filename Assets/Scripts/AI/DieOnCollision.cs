using UnityEngine;

namespace AI
{
    internal class DieOnCollision : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var core = GetComponent<NPCCore>();
            core.enabled = true;
            core.ForceDeath();
        }
    }
}
