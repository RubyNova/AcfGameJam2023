using Environment;
using UnityEngine;
using UnityEngine.Events;

namespace SetPieceHelpers
{
    public class SpecificEntityWaitTrigger : RoomEntityBehaviour
    {
        [SerializeField]
        private RoomEntityBehaviour _target;

        public UnityEvent TargetArrived;

        private void Awake()
        {
            enabled = false;
        }

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            enabled = isActiveRoom;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_target.gameObject != collision.gameObject)
            {
                return;
            }

            TargetArrived.Invoke();
        }
    }
}
