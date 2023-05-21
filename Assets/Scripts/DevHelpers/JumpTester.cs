using Environment;
using Movement;
using SetPieceHelpers.Paranoia;
using UnityEngine;

namespace DevHelpers
{
    public class JumpTester : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private GroundMover _mover;

        [SerializeField]
        private JumpNode _testNode;

        [SerializeField]
        private Collider2D _triggerToDisableOnLaunch;

        private bool _active = false;
        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _active = isActiveRoom;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_active || !collision.gameObject.CompareTag("Player"))
            {
                return;
            }

            _mover.ApplyRawVelocity(_testNode.CalculateJumpForce(transform.position));

            enabled = false;
            _triggerToDisableOnLaunch.enabled = false;
        }
    }
}
