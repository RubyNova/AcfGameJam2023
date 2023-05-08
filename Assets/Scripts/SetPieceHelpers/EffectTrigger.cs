using Environment;
using UnityEngine;
using UnityEngine.Events;

namespace SetPieceHelpers
{
    public class EffectTrigger : RoomEntityBehaviour
    {
        private bool _isActive = false;

        public UnityEvent EffectTriggered;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _isActive = isActiveRoom;
        }

        public void Trigger()
        {
            if (!_isActive)
            {
                return;
            }

            EffectTriggered.Invoke();
        }
    }
}
