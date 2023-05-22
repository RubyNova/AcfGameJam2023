using Environment;
using Movement;
using Player;
using SetPieceHelpers.BossFightHelpers;
using System;
using UnityEngine;

namespace SetPieceHelpers.Rage
{
    public class RageNPCCore : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private GroundMover _mover;

        [Header("Configuration"), SerializeField]
        private float _movementSpeed;

        private PlayerController _playerController;
        private JumpNode _currentJumpNode;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
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

            if (!_mover.IsGrounded)
            {
                return;
            }

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
    }
}
