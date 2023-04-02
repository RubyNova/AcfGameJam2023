using UnityEngine;

namespace Environment
{
    public abstract class RoomEntityBehaviour : MonoBehaviour
    {
        public abstract void NotifyActiveStatus(bool isActiveRoom);
    }
}
