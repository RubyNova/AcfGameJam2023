using Player;
using UnityEngine;

namespace Environment
{
    public class Entrance : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private Room _owningRoom;

        [Header("Configuration"), SerializeField]
        private int _entranceExitId;

        public Room OwningRoom => _owningRoom;
        public int EntranceExitId => _entranceExitId;

        public void MovePlayerIntoRoom(PlayerController player)
        {
            player.transform.position = transform.position;
            _owningRoom.BecomeActiveRoom(transform.position);
        }
    }
}