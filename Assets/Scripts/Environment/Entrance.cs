using Player;
using UnityEngine;

namespace Environment
{
    public class Entrance : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private Room _owningRoom;

        public Room OwningRoom => _owningRoom;

        public void MovePlayerIntoRoom(PlayerController player)
        {
            player.transform.position = transform.position;
            _owningRoom.BecomeActiveRoom(transform.position);
        }
    }
}