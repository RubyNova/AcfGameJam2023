using Environment;
using UnityEngine;

namespace SetPieceHelpers
{
    public class DoorToggler : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private Exit[] _exitsToToggle;

        [SerializeField]
        private GameObject _doorRenderObject;

        [SerializeField]
        private bool _hasDoorRenderObject;

        private bool _isActive;

        private void Awake()
        {
            _isActive = false;
            if (!_hasDoorRenderObject)
            {
                return;
            }

            _doorRenderObject.SetActive(false);
        }

        public void ChangeDoorState(bool newState)
        {
            if (!_isActive)
            {
                return;
            }
            
            foreach (var exit in _exitsToToggle)
            {
                exit.gameObject.SetActive(newState);
            }

            if (!_hasDoorRenderObject)
            {
                return;
            }

            _doorRenderObject.SetActive(newState);
        }

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _isActive = isActiveRoom;
        }
    }
}
