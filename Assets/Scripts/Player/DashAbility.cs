using System.Collections;
using UnityEngine;

namespace Player
{
    public class DashAbility : PlayerAbilityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private string _groundTagName;

        [Header("Configuration"), SerializeField]
        private float _dashSpeed;

        [SerializeField]
        private float _dashDuration;

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
            _hasBeenUsed = true;
            context.AnimPipe.PerformDashAnim();

            while (timeRemaining > 0)
            {
                context.Mover.ApplyRawDirection(new(context.PlayerInput.InputAxes.x * _dashSpeed, 0));
                yield return null;
                timeRemaining -= Time.deltaTime;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag(_groundTagName))
            {
                return;
            }

            _hasBeenUsed = false;
        }
    }
}
