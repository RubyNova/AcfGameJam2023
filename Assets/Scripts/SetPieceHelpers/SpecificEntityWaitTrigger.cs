using Environment;
using UnityEngine;
using UnityEngine.Events;

namespace SetPieceHelpers
{
    public class SpecificEntityWaitTrigger : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private RoomEntityBehaviour _target;

        private bool _active = false;
        
        public UnityEvent TargetArrived;

        private void Awake()
        {
            _active = false;
        }

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _active = isActiveRoom;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_target.gameObject != collision.gameObject)
            {
                return;
            }

            TargetArrived.Invoke();
            _active = false;
        }
    }
}
