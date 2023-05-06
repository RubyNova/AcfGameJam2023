using System.Collections;
using UnityEngine;

namespace Player
{
    public class DoubleJumpAbility : PlayerAbilityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private string _groundTagName;

        private bool _hasBeenUsed;

        private void Awake()
        {
            Name = "DoubleJump";
            _hasBeenUsed = false;
        }

        protected override bool ValidateExecution(PlayerContextObject context)
        {
            return !context.Mover.IsGrounded && !_hasBeenUsed && !context.Controller.MovementIsOverridden;
        }

        protected override IEnumerator ExecuteAbility(PlayerContextObject context)
        {
            _hasBeenUsed = true;
            context.ForceJump = true;
            yield break;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag(_groundTagName))
            {
                return;
            }

            _hasBeenUsed = false;
        }

        protected override void EnforceInputDefaults()
        {
            _inputTrigger = new InputInfo(Vector2.zero, false, true, false, false);
            _inputTrigger.UpdateInputMonitoringFlags(false, false, false, false, false, true);
        }
    }
}
