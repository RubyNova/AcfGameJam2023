using Movement;
using UnityEngine;

namespace AI
{
    internal class DieOnCollision : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var core = GetComponent<NPCCore>();
            core.enabled = true;
            GetComponent<GroundMover>().ApplyMove(Vector2.zero, 0, false, 0);
            core.ForceDeath();
        }
    }
}
