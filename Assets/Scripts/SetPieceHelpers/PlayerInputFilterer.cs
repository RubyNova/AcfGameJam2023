using Environment;
using Player;
using UnityEngine;

namespace SetPieceHelpers
{
    public class PlayerInputFilterer : RoomEntityBehaviour
    {
        private bool _isActive = false;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _isActive = isActiveRoom;
        }

        public void Enable()
        {
            if (!_isActive)
            {
                return;
            }

            PlayerDeviceInputManager.Instance.EnableFiltering();
        }

        public void Disable()
        {
            if (!_isActive)
            {
                return;
            }

            PlayerDeviceInputManager.Instance.DisableFiltering();   
        }
    }
}
