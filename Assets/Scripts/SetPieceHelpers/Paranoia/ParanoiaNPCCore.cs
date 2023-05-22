using Environment;
using Movement;
using Player;
using SetPieceHelpers.BossFightHelpers;
using System;
using UnityEngine;

namespace SetPieceHelpers.Paranoia
{
    public class ParanoiaNPCCore : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private string _playerTag = "Player";

        [SerializeField]
        private GroundMover _mover;

        [SerializeField]
        private AnimationDataPipe _pipe;

        [Header("Configuration"), SerializeField]
        private PlatformNode[] _platforms;

        [SerializeField]
        private float _movementSpeed;

        [SerializeField]
        private float _movementCooldownPeriod;

        [SerializeField]
        private int _damageToDealOnHit;

        private PlayerController _playerController;
        private PlatformNode _platformNode;
        private JumpNode _jumpNode;
        private float _movementCooldownRemaining = 0;
        private Transform _targetPosition;
        private PlatformNode _targetPlatform;
        private JumpNode _targetNode;
        private bool _selfPlatformResolved = true;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            if (!isActiveRoom)
            {
                _playerController = null;
                enabled = false;
            }
        }

        private void Update()
        {
            if (_playerController == null) // failsafe just in case! :D
            {
                enabled = false;
                return;
            }

            if (!Mathf.Approximately(_movementCooldownRemaining, 0))
            {
                _movementCooldownRemaining -= Time.deltaTime;
                return;
            }

            _movementCooldownRemaining = 0;

            if (!_mover.IsGrounded)
            {
                return;
            }

            if (!VerifyTargetPlatformIsStillValid())
            {
                _targetPosition = null;
                _targetPlatform = null;
                _targetNode = null;
                _selfPlatformResolved = false;
            }
            else
            {
                WalkTowardsJumpNodeTarget();
                return;
            }

            if (!_selfPlatformResolved)
            {
                var playerPosition = _playerController.transform.position;

                bool playerIsOnPlatform = false;
                PlatformNode playerPlatform = null;

                foreach (var platform in _platforms)
                {
                    if (platform.PlayerIsHere)
                    {
                        playerIsOnPlatform = true;
                        playerPlatform = platform;
                        break;
                    }
                }

                if (playerIsOnPlatform)
                {
                    bool isSelfOnRightHandNode = _jumpNode == _platformNode.RightTarget;
                    bool isPlayerPlatformToTheLeft = playerPlatform.transform.position.x < _platformNode.transform.position.x;

                    if (isPlayerPlatformToTheLeft && isSelfOnRightHandNode)
                    {
                        ResolveNodeNotification();
                    }
                    else
                    {
                        ResolveJumpTarget(playerPlatform);
                    }
                }
                else
                {
                    // player is on the ground, just walk off the edge.
                    WalkTowardsPlayer();
                    ResolveNodeNotification();
                }
            }

            WalkTowardsPlayer();

            void WalkTowardsPlayer()
            {
                bool shouldWalkLeft = _playerController.transform.position.x < transform.position.x;
                _mover.ApplyMove(new(shouldWalkLeft ? -1 : +1, 0), _movementSpeed, false, 0);
            }

            void WalkTowardsJumpNodeTarget()
            {
                bool shouldWalkLeft = _targetNode.transform.position.x < transform.position.x;
                _mover.ApplyMove(new(shouldWalkLeft ? -1 : +1, 0), _movementSpeed, false, 0);
            }

            void ResolveNodeNotification()
            {
                _selfPlatformResolved = true;
            }

            void DoJump(JumpNode jumpTarget)
            {
                _mover.ApplyRawVelocity(jumpTarget.CalculateJumpForce(transform.position));
            }

            bool VerifyTargetPlatformIsStillValid()
            {
                return _targetPlatform != null && _targetPlatform.PlayerIsHere;
            }

            void ResolveJumpTarget(PlatformNode playerPlatform)
            {
                PlatformNode closestPlatform = null;
                float smallestDistance = float.PositiveInfinity;
                float testDistance = 0;

                foreach (var platform in _platformNode.RelatedPlatforms)
                {
                    if (platform == playerPlatform)
                    {
                        closestPlatform = platform;
                        break;
                    }

                    testDistance = Vector2.Distance(_platformNode.transform.position, platform.transform.position);

                    if (smallestDistance > testDistance)
                    {
                        smallestDistance = testDistance;
                        closestPlatform = platform;
                    }
                }

                var (jumpInstruction, jumpTarget) = closestPlatform.DetermineCorrectJumpNodeBasedOnOrigin(transform.position);

                switch (jumpInstruction)
                {
                    case PlatformNode.JumpNodeDeterminationResult.Failed:
                        throw new InvalidOperationException($"Paranoia NPC Core could not calculate a path to the player given the current configuration. Offending platform: {closestPlatform.name}");
                    case PlatformNode.JumpNodeDeterminationResult.RequiresRepositionToTheLeft:
                        _targetPosition = closestPlatform.RepositionLocationLeft;
                        _targetPlatform = closestPlatform;
                        _targetNode = jumpTarget;
                        break;
                    case PlatformNode.JumpNodeDeterminationResult.RequiresRepositionToTheRight:
                        _targetPosition = closestPlatform.RepositionLocationRight;
                        _targetPlatform = closestPlatform;
                        _targetNode = jumpTarget;
                        break;
                    case PlatformNode.JumpNodeDeterminationResult.Success:
                        DoJump(jumpTarget);
                        break;
                }

                ResolveNodeNotification();
            }
        }

        public void BeginFight()
        {
            _playerController = PlayerController.Instance;
            enabled = true;
        }

        public void NotifyJumpNodeReached(JumpNode node, PlatformNode owningPlatform, ParanoiaBossDetector detector)
        {
            _selfPlatformResolved = false;
            _jumpNode = node;
            _platformNode = owningPlatform;
            transform.position = detector.transform.position;
        }

        public void NotifyPlatformChanged(PlatformNode platform)
        {
            _platformNode = platform;
        }

        public void ForceMovementCooldown()
        {
            _movementCooldownRemaining = _movementCooldownPeriod;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag(_playerTag) || !Mathf.Approximately(_movementCooldownRemaining, 0))
            {
                return;
            }

            PlayerController.Instance.GetComponent<PlayerHealthController>().AdjustHealth(_damageToDealOnHit);

            _movementCooldownRemaining = _movementCooldownPeriod;
        }
    }
}
