using Environment;
using Movement;
using Player;
using UnityEngine;

namespace SetPieceHelpers
{
    public class PlayerInputSequencer : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private GroundMover _mover;

        [SerializeField]
        private AnimationDataPipe _pipe;

        [Header("Configuration"), SerializeField]
        private bool _useOverrideMovement;

        [SerializeField]
        private float _movementSpeed;

        [SerializeField]
        private float _jumpForce;

        [SerializeField]
        private bool _performHurtAnim;

        [SerializeField]
        private bool _performDieAnim;

        [SerializeField]
        private InputInfo[] _inputs;

        private InputInfo _currentInput = null;
        private int _currentIndex = -1;

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            if (_currentInput == null)
            {
                return;
            }

            _pipe.InputBasedUpdates(_currentInput);

            if (_useOverrideMovement)
            {
                _mover.ApplyRawDirection(_currentInput.InputAxes * _movementSpeed);
            }
            else
            {
                var filteredAxes = _currentInput.InputAxes;

                if (Mathf.Approximately(-1, filteredAxes.y))
                {
                    filteredAxes.y = 0;
                }

                _mover.ApplyMove(_currentInput.InputAxes, _movementSpeed, _currentInput.JumpInput, _jumpForce);
            }
        }

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            enabled = isActiveRoom;
        }

        public void MoveToNextInput()
        {
            if (!enabled)
            {
                return;
            }

            _currentIndex++;
            
            if (_currentIndex >= _inputs.Length)
            {
                _currentIndex = 0;
            }

            _currentInput = _inputs[_currentIndex];
        }
    }
}
