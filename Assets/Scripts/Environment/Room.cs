using UnityEngine;
using Cinemachine;

namespace Environment
{
    public class Room : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private RoomEntityBehaviour[] _ownedEntities;

        [SerializeField]
        private AreaController _owningAreaState;

        public AreaController OwningAreaState => _owningAreaState;

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
            //activate the cinemachine camera child
            foreach (Transform child in transform)
            {
                if (child.GetComponent<CinemachineVirtualCamera>() != null)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        public void BecomeInactiveRoom()
        {
            NotifyRoomEntities(false);
            //deactivate the cinemachine camera child
            foreach (Transform child in transform)
            {
                if (child.GetComponent<CinemachineVirtualCamera>() != null)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
