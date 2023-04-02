using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Environment
{
    public class Exit : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private Room _owningRoom;

        [SerializeField]
        private Entrance _connectedEntrance;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent<PlayerController>(out var controller))
            {
                return;
            }

            _connectedEntrance.MovePlayerIntoRoom(controller);
            _owningRoom.BecomeInactiveRoom();
        }
    }
}
