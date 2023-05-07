using AI;
using Movement;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    class RageAbility : PlayerAbilityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private LayerMask _enemyLayer;
        
        [Header("Configuration"), SerializeField]
        private float _pushBackRadius = 2;

        [SerializeField]
        private float _pushForce = 7;

        [SerializeField]
        private float _animationWindow = 0.3f;

        private float _animationWindowRemaining;

        private void Awake()
        {
            _animationWindowRemaining = _animationWindow;
            Name = "Rage";
        }

        private void Update()
        {
            if (_animationWindowRemaining <= 0)
            {
                return;
            }

            _animationWindowRemaining -= Time.deltaTime;
        }

        protected override bool ValidateExecution(PlayerContextObject context)
        {
            return _animationWindowRemaining <= 0;
        }

        protected override IEnumerator ExecuteAbility(PlayerContextObject context)
        {
            var enemies = Physics2D.OverlapCircleAll(transform.position, _pushBackRadius, _enemyLayer);

            if (enemies.Length == 0)
            {
                yield break;
            }

            foreach(var enemy in enemies)
            {
                var direction = ((Vector2)enemy.transform.position - (Vector2)transform.position).normalized;

                if(!enemy.TryGetComponent<NPCCore>(out var core))
                {
                    continue;
                }

                core.enabled = false;
                enemy.GetComponent<GroundMover>().ApplyRawDirection(direction * _pushForce);
                enemy.AddComponent<DieOnCollision>(); 
            }
        }

        protected override void EnforceInputDefaults()
        {
            _inputTrigger = new InputInfo(Vector2.zero, false, false, true, false);
            _inputTrigger.UpdateInputMonitoringFlags(false, false, false, false, true, false);
        }
    }
}