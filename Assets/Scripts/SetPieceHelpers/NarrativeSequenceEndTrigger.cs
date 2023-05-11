using Environment;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace SetPieceHelpers
{
    public class NarrativeSequenceEndTrigger : RoomEntityBehaviour
    {
        public UnityEvent onSequenceEnd;

        private UnityAction onSequenceEndAction;

        private bool _subscribed = false;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            onSequenceEndAction ??= new UnityAction(() => onSequenceEnd.Invoke());

            if (isActiveRoom)
            {
                _subscribed = true;
                MenuController.Instance.NarrativeMenu.sequenceFinishedEvent.AddListener(onSequenceEndAction);
            }
            else if (_subscribed)
            {
                _subscribed = false;
                MenuController.Instance.NarrativeMenu.sequenceFinishedEvent.RemoveListener(onSequenceEndAction);
            }
        }
    }
}
