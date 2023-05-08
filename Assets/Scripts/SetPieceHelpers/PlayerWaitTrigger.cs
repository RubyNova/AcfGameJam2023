using Environment;
using UnityEngine;
using UnityEngine.Events;

namespace SetPieceHelpers
{
    public class PlayerWaitTrigger : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private string _playerTagName = "Player";

        private bool _isActive = false;

        public UnityEvent PlayerArrived;

        private void Awake()
        {
            _isActive = false;
        }

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _isActive = isActiveRoom;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(_playerTagName))
            {
                return;
            }

            PlayerArrived.Invoke();
            _isActive = false;
        }
    }
}
