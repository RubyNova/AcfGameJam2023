using UnityEngine;

namespace Environment
{
    public class Room : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private RoomEntityBehaviour[] _ownedEntities;

        private void Awake()
        {
            BecomeInactiveRoom();
        }

        private void NotifyRoomEntities(bool isActiveRoom)
        {
            foreach (var entity in _ownedEntities)
            {
                entity.NotifyActiveStatus(isActiveRoom);
            }
        }

        public void BecomeActiveRoom()
        {
            NotifyRoomEntities(true);
        }

        public void BecomeInactiveRoom()
        {
            NotifyRoomEntities(false);
        }
    }
}
