using Environment;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace SetPieceHelpers
{
    public class NarrativeSequenceEndTrigger : RoomEntityBehaviour
    {
        public UnityEvent onSequenceEnd;

        private UnityAction _onSequenceEndAction;

        private bool _subscribed = false;

        private bool _isActive = false;

        private void Awake()
        {
            _onSequenceEndAction = new UnityAction(() =>
            {
                UnhookFromNarrativeControls();
                onSequenceEnd.Invoke();
            });
        }

        private void UnhookFromNarrativeControls()
        {
            _subscribed = false;
            MenuController.Instance.NarrativeMenu.sequenceFinishedEvent.RemoveListener(_onSequenceEndAction);
        }

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _isActive = isActiveRoom;

            if (!_isActive && _subscribed)
            {
                UnhookFromNarrativeControls();
            }
        }

        public void WaitForNextSequenceEnd()
        {
            if (!_isActive || _subscribed)
            {
                return;
            }

            _subscribed = true;
            MenuController.Instance.NarrativeMenu.sequenceFinishedEvent.AddListener(_onSequenceEndAction);
        }
    }
}
