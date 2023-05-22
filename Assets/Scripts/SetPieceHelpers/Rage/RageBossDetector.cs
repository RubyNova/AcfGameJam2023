using Environment;
using SetPieceHelpers.BossFightHelpers;
using UnityEngine;

namespace SetPieceHelpers.Rage
{
    public class RageBossDetector : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private JumpNode _nodeToControl;

        private bool _active = false;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _active = isActiveRoom;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_active || !collision.gameObject.TryGetComponent<RageNPCCore>(out var core))
            {
                return;
            }

            core.NotifyJumpNodeReached(_nodeToControl);
        }
    }
}
