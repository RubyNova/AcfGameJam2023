using AI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

namespace Player
{
    class RageAbility : PlayerAbilityBehaviour
    {

        [Header("Dependencies"), SerializeField]
        private LayerMask _enemyLayer;
        
        [Header("Configuration"), SerializeField]
        private float _pushBackRadius = 2;

        [SerializeField]
        private string _groundLayerTag = "Ground";

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
            HasBeenConsumedUntilNextRest = true;
            var enemies = Physics2D.OverlapCircleAll(transform.position, _pushBackRadius, _enemyLayer);

            foreach(var enemy in enemies)
            {
                var direction = ((Vector2)enemy.transform.position - (Vector2)transform.position).normalized;

                if(!enemy.TryGetComponent<NPCCore>(out var core))
                {
                    continue;
                }

                enemy.GetComponent<Rigidbody2D>().velocity = direction * _pushForce;
                core.enabled = false;
                enemy.AddComponent<DieOnCollision>(); 
            }

            yield return null;
        }
    }
}