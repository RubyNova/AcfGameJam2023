using Movement;
using UnityEngine;

namespace Player
{
    public class AnimationDataPipe : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private GroundMover _mover;

        [SerializeField]
        private Animator _animator;

        public void InputBasedUpdates(InputInfo input)
        {
            _animator.SetBool("IsGrounded", _mover.IsGrounded);
            _animator.SetFloat("Speed", Mathf.Abs(input.InputAxes.x));
            _animator.SetBool("IsJumping", !_mover.IsGrounded);
            _animator.SetBool("IsCrawling", Mathf.Approximately(input.InputAxes.y, -1) && _mover.IsGrounded);
            _animator.SetFloat("YAxisVelocity", _rigidbody.velocity.y);
        }

        public void PerformDieAnim()
        {
            _animator.SetBool("IsDead", true);
        }
    }
}
