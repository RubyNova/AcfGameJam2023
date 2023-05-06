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
            if (context.Mover.IsGrounded)
            {
                _hasBeenUsed = false;
            }

            return !_hasBeenUsed;
        }

        protected override IEnumerator ExecuteAbility(PlayerContextObject context)
        {
            float timeRemaining = _dashDuration;
            _hasBeenUsed = true;
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
    }
}
