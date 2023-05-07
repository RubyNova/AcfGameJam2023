using UnityEngine;

namespace AI
{
    public class NPCAnimationDataPipe : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private Animator _animator;

        public bool IsRunning { get; set; }

        public void PerformAlertedAnim()
        {
            _animator.SetTrigger("Alerted");
        }

        public void PerformAttackAnim()
        {
            _animator.SetTrigger("Attack");
        }

        public void PerformDieAnim()
        {
            _animator.SetTrigger("Dead");
        }
    }
}
