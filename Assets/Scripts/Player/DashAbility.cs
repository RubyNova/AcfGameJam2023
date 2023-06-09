using System.Collections;
using UnityEngine;

namespace Player
{
    public class DashAbility : PlayerAbilityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private string _groundTagName;

        [Header("Configuration"), SerializeField]
        private float _dashSpeed = 30;

        [SerializeField]
        private float _dashDuration = 0.2f;

        private bool _hasBeenUsed;

        private void Awake()
        {
            Name = "Dash";
            _hasBeenUsed = false;
        }

        protected override bool ValidateExecution(PlayerContextObject context)
        {
            return !_hasBeenUsed;
        }

        protected override IEnumerator ExecuteAbility(PlayerContextObject context)
        {
            float timeRemaining = _dashDuration;

            if (!context.Mover.IsGrounded)
            {
                _hasBeenUsed = true;
            }

            context.AnimPipe.PerformDashAnim();

            while (timeRemaining > 0)
            {
                context.Mover.ApplyRawDirection(new(context.PlayerInput.InputAxes.x * _dashSpeed, 0));
                yield return null;
                timeRemaining -= Time.deltaTime;
            }
        }

        protected override void EnforceInputDefaults()
        {
            _inputTrigger = new InputInfo(Vector2.zero, false, true, false, false);
            _inputTrigger.UpdateInputMonitoringFlags(false, false, false, true, false, false);
            _overridesNormalMovement = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(_groundTagName))
            {
                return;
            }

            _hasBeenUsed = false;
        }
    }
}
