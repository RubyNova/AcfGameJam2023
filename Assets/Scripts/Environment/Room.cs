using UnityEngine;
using Cinemachine;

namespace Environment
{
    public class Room : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private RoomEntityBehaviour[] _ownedEntities;

        [SerializeField]
        private AreaState _owningAreaState;

        public AreaState OwningAreaState => _owningAreaState;

        public Vector2 LastReportedPlayerPosition { get; set; }

        private void Awake()
        {
            BecomeInactiveRoom();
        }

        private void NotifyRoomEntities(bool isActiveRoom)
        {
            foreach (var entity in _ownedEntities)
            {
                entity.NotifyActiveStatus(isActiveRoom, this);
            }
        }

        public void BecomeActiveRoom(Vector2 playerEntryLocation)
        {
            LastReportedPlayerPosition = playerEntryLocation;
            _owningAreaState.IsActiveArea = true;
            NotifyRoomEntities(true);
        }

        public void BecomeInactiveRoom()
        {
            NotifyRoomEntities(false);
        }
    }
}
