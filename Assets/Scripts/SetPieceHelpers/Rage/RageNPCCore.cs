using Environment;
using Movement;
using Player;
using SetPieceHelpers.BossFightHelpers;
using System;
using System.Collections;
using UnityEngine;

namespace SetPieceHelpers.Rage
{
    public class RageNPCCore : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private string _playerTagName = "Player";

        [SerializeField]
        private GroundMover _mover;

        [SerializeField]
        private Collider2D _rageDamageCollider;

        [SerializeField]
        private BossDefeatedTrigger _defatedTrigger;

        [Header("Configuration"), SerializeField]
        private float _movementSpeed = 10; // arbitrary default value so it works

        [SerializeField]
        private int _damageOnRageAndCollision = 10; // arbitrary default value so it works

        [SerializeField]
        private float _rageWindUpTime = 1;

        [SerializeField]
        private float _rageColliderActiveTime = 0.2f;

        [SerializeField]
        private float _rageAbilityCooldown = 2;

        [SerializeField]
        private float _playerDistanceThreshold = 0.5f;

        [SerializeField]
        private float _damageTakenMovementCooldown = 0.5f;

        [SerializeField]
        private int _itemHitsRequiredForDefeat = 5;

        private PlayerController _playerController;
        private JumpNode _currentJumpNode;
        private float _rageWindUpRemaining = 0;
        private float _rageColliderActiveRemaining = 0;
        private float _rageAbilityCooldownRemaining = 0;
        private float _damageTakenMovementCooldownRemaining = 0;
        private float _itemHitsRemaining;
        private bool _forceEarlyRageRoutineTermination = false;
        private Coroutine _rageRoutine;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _itemHitsRemaining = _itemHitsRequiredForDefeat;
            if (!isActiveRoom)
            {
                _playerController = null;
                enabled = false;
            }
        }

        public void NotifyJumpNodeReached(JumpNode node)
        {
            _currentJumpNode = node;
        }

        private void Update()
        {
            if (_playerController == null) // failsafe just in case! :D
            {
                enabled = false;
                return;
            }

            if (!_mover.IsGrounded || _rageRoutine != null || !Mathf.Approximately(_damageTakenMovementCooldownRemaining, 0))
            {
                return;
            }

            if (_rageAbilityCooldown >= 0)
            {
                _rageAbilityCooldownRemaining -= Time.deltaTime;
            }
            else
            {
                _rageAbilityCooldownRemaining = 0;
            }

            if ((Vector2.Distance(transform.position, _playerController.transform.position) <= _playerDistanceThreshold) && Mathf.Approximately(_rageAbilityCooldownRemaining, 0))
            {
                _rageRoutine = StartCoroutine(DoRageAbilityCycle());
            }
            else
            {
                if (_currentJumpNode != null)
                {
                    _mover.ApplyRawVelocity(_currentJumpNode.CalculateJumpForce(transform.position));
                }
                else
                {
                    bool shouldWalkLeft = _playerController.transform.position.x < transform.position.x;
                    _mover.ApplyMove(new(shouldWalkLeft ? -1 : +1, 0), _movementSpeed, false, 0);
                }
            }

            IEnumerator DoRageAbilityCycle()
            {
                _rageWindUpRemaining = _rageWindUpTime;
                _rageColliderActiveRemaining = _rageColliderActiveTime;
                _rageAbilityCooldownRemaining = _rageAbilityCooldown;

                yield return new WaitForSeconds(1);

                while (!Mathf.Approximately(_rageWindUpRemaining, 0))
                {
                    _rageWindUpRemaining -= Time.deltaTime;

                    if (_forceEarlyRageRoutineTermination)
                    {
                        EndRoutine();
                        yield break;
                    }

                    yield return null;
                }

                _rageDamageCollider.enabled = true;

                while (!Mathf.Approximately(_rageColliderActiveRemaining, 0))
                {
                    _rageColliderActiveRemaining -= Time.deltaTime;
                    
                    if (_forceEarlyRageRoutineTermination)
                    {
                        EndRoutine();
                        _rageDamageCollider.enabled = false;
                        yield break;
                    }

                    yield return null;
                }

                void EndRoutine()
                {
                    _rageWindUpRemaining = 0;
                    _rageColliderActiveRemaining = 0;
                    _rageDamageCollider.enabled = false;
                    _rageRoutine = null;
                }
            }
        }

        public void StartBossFight()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<DisposableItem>(out var item) && item.Config.IsHeavyItem)
            {
                _itemHitsRemaining--;

                if (_itemHitsRemaining == 0)
                {
                    _playerController = null;
                    _defatedTrigger.EndBossBattle();
                    _forceEarlyRageRoutineTermination = true;
                }
            }
            else if (collision.gameObject.CompareTag(_playerTagName))
            {
                PlayerController.Instance.GetComponent<PlayerHealthController>().AdjustHealth(_damageOnRageAndCollision);
                _forceEarlyRageRoutineTermination = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(_playerTagName))
            {
                PlayerController.Instance.GetComponent<PlayerHealthController>().AdjustHealth(_damageOnRageAndCollision);
                _forceEarlyRageRoutineTermination = true;
            }
        }
    }
}
