using Environment;
using UnityEngine;
using UnityEngine.Events;

namespace SetPieceHelpers.BossFightHelpers
{
    public class BossDefeatedTrigger : RoomEntityBehaviour
    {
        private bool _active = false;
        private bool _hasBeenInvoked = false;

        public UnityEvent BossBattleEnded;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _active = isActiveRoom;
        }

        public void EndBossBattle()
        {
            if (!_active || _hasBeenInvoked)
            {
                return;
            }

            BossBattleEnded.Invoke();
            _hasBeenInvoked = true;
        }
    }
}
