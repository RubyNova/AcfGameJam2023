using UnityEngine;

namespace Environment
{
    public abstract class RoomEntityBehaviour : MonoBehaviour
    {
        public abstract void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default);
    }
}
