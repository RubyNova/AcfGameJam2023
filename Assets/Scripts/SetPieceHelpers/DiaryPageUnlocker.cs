using Environment;
using UI;
using UnityEngine;

namespace SetPieceHelpers
{
    public class DiaryPageUnlocker : RoomEntityBehaviour
    {
        [Header("Configuration"), SerializeField]
        private int _diaryPageIndexToUnlock;

        private bool _active = false;
        private bool _pageIsUnlocked = false;
        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _active = isActiveRoom;
        }

        public void UnlockPage()
        {
            if (!_active || _pageIsUnlocked)
            {
                return;
            }

            MenuController.Instance.DiaryPageUnlockerControl.EnableDiaryPage(_diaryPageIndexToUnlock);
            _pageIsUnlocked = true;
        }
    }
}
