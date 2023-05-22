using Environment;
using UI;
using UnityEngine;

namespace SetPieceHelpers
{
    public class TutorialPanelPresenter : RoomEntityBehaviour
    {
        [SerializeField]
        private int _tutorialPanelIndex;

        private bool _active = false;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _active = isActiveRoom;
        }

        public void ShowPanel()
        {
            if (!_active)
            {
                return;
            }
            
            MenuController.Instance.DialoguePlayer.EnableDialoguePage(_tutorialPanelIndex);
        }

        public void HidePanel()
        {
            if (!_active)
            {
                return;
            }
            
            MenuController.Instance.DialoguePlayer.DisableDialoguePage(_tutorialPanelIndex);
        }
    }
}
