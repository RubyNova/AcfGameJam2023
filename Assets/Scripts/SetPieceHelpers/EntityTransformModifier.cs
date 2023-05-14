using Environment;
using UnityEngine;

namespace SetPieceHelpers
{
    public class EntityTransformModifier : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private Transform _targetTransform;

        [Header("Configuration"), SerializeField]
        private Vector2 _positionDiff;

        [SerializeField]
        private Vector3 _rotationDiff;

        [SerializeField]
        private bool _isDirectModification = false;

        private bool _isActive = false;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _isActive = isActiveRoom;
        }

        public void ApplyModification()
        {
            if (!_isActive)
            {
                return;
            }

            if (_isDirectModification)
            {
                _targetTransform.position = _positionDiff;
                _targetTransform.rotation = Quaternion.Euler(_rotationDiff);
            }
            else
            {
                _targetTransform.Translate(_positionDiff.x, _positionDiff.y, 0);
                _targetTransform.Rotate(_rotationDiff);
            }
        }
    }
}
