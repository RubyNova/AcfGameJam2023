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

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            onSequenceEndAction ??= new UnityAction(() => onSequenceEnd.Invoke());

            if (isActiveRoom)
            {
                MenuController.Instance.NarrativeMenu.sequenceFinishedEvent.AddListener(onSequenceEndAction);
            }
            else
            {
                MenuController.Instance.NarrativeMenu.sequenceFinishedEvent.RemoveListener(onSequenceEndAction);
            }
        }
    }
}
