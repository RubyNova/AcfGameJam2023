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

        public bool HasBeenConsumedUntilNextRest { get; set; }


        private void Awake()
        {
            Name = "Rage";
            HasBeenConsumedUntilNextRest = false;
        }

        protected override bool ValidateExecution(PlayerContextObject context)
        {
            return !HasBeenConsumedUntilNextRest;
        }

        protected override IEnumerator ExecuteAbility(PlayerContextObject context)
        {
            var enemies = Physics2D.OverlapCircleAll(transform.position, _pushBackRadius, _enemyLayer);

            if (enemies.Length == 0)
            {
                yield break;
            }

            HasBeenConsumedUntilNextRest = true;

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
    }
}