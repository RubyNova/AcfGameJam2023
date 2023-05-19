using Environment;
using Movement;
using Player;
using UnityEngine;

namespace SetPieceHelpers.Paranoia
{
    public class ParanoiaNPCCore : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private GroundMover _mover;

        [SerializeField]
        private AnimationDataPipe _pipe;

        [Header("Configuration"), SerializeField]
        private JumpNode[] _jumpLocations;

        PlayerController _playerController;

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

            if (!_mover.IsGrounded)
            {
                return;
            }

            var jumpNode = _jumpLocations[0];
            var nodePosition = jumpNode.transform.position;
            var selfPosition = transform.position;
            
            float horizontalValue = selfPosition.x - nodePosition.x;

            if ((horizontalValue > 0 && !Mathf.Approximately(transform.right.x, -1)) || (horizontalValue < 0 && !Mathf.Approximately(transform.right.x, 1)))
            {
                transform.Rotate(0, 180, 0);

            }

            Vector2 jumpForce = jumpNode.CalculateJumpForce(selfPosition);

            _mover.ApplyRawVelocity(jumpForce);
        }

        public void BeginFight()
        {
            _playerController = PlayerController.Instance;
            enabled = true;
        }
    }
}
