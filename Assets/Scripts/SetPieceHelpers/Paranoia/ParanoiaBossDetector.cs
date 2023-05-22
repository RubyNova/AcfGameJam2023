using Environment;
using UnityEngine;

namespace SetPieceHelpers.Paranoia
{
    public class ParanoiaBossDetector : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private JumpNode _nodeToControl;

        [SerializeField]
        private PlatformNode _owningPlatform;

        private bool _active = false;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _active = isActiveRoom;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_active || !collision.gameObject.TryGetComponent<ParanoiaNPCCore>(out var core))
            {
                return;
            }

            core.NotifyJumpNodeReached(_nodeToControl, _owningPlatform, this);
        }
    }
}
