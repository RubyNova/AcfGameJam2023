using Environment;
using UnityEngine;

namespace SetPieceHelpers
{
    public class EntityTransformModifier : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private Transform _targetTransform;

        [SerializeField]
        private Vector2 _positionDiff;

        [SerializeField]
        private Vector3 _rotationDiff;

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

            _targetTransform.Translate(_positionDiff.x, _positionDiff.y, 0);
            _targetTransform.Rotate(_rotationDiff);
        }
    }
}
