using Environment;
using UnityEngine.Events;
using UnityEngine;

namespace SetPieceHelpers
{
    public class SpecificEntityWaitPhysicalCollision : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private RoomEntityBehaviour _target;

        private bool _active = false;
        
        public UnityEvent TargetCollided;

        private void Awake()
        {
            _active = false;
        }

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _active = isActiveRoom;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_target.gameObject != collision.gameObject)
            {
                return;
            }

            TargetCollided.Invoke();
            _active = false;
        }
    }
}
