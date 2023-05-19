using Environment;
using SetPieceHelpers.Pocos;
using UnityEngine;
using UnityEngine.Events;

namespace SetPieceHelpers
{
    public class InvocationListAppender : RoomEntityBehaviour
    {
        private bool _active = false;

        public UnityEvent InvocationList;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _active = isActiveRoom;
        }

        public void AppendInvocationList()
        {
            if (!_active)
            {
                return;
            }

            SetPieceActionQueue.Instance.Enqueue(() => InvocationList.Invoke());
        }
    }
}
