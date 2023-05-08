using AI;
using Environment;
using Movement;
using Player;
using UnityEngine;

namespace SetPieceHelpers
{
    public class InputStreamer : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private GroundMover _targetMover;

        [SerializeField]
        private AnimationDataPipe _playerAnimPipe;

        [SerializeField]
        private NPCAnimationDataPipe _npcAnimPipe;

        [Header("Configuration"), SerializeField]
        private bool _hasPlayerAnimations;

        [SerializeField]
        private bool _hasNpcAnimations;

        [SerializeField]
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
        private bool _enableNpcRun;

        [SerializeField]
        private bool _enableNpcKnockbackAnim;

        [SerializeField]
        private bool _enableNpcAlertedAnim;

        [SerializeField]
        private bool _enableNpcAttackAnim;

        [SerializeField]
        private InputInfo _inputToStream;

        private void Awake()
        {
            enabled = false;
        }

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            enabled = isActiveRoom;
        }

        private void Update()
        {
            if (_useOverrideMovement)
            {
                _targetMover.ApplyMove(_inputToStream.InputAxes, _movementSpeed, _inputToStream.JumpInput, _jumpForce);
            }
            else
            {
                _targetMover.ApplyRawDirection(_inputToStream.InputAxes * _movementSpeed);
            }

            if (_hasPlayerAnimations)
            {
                _playerAnimPipe.InputBasedUpdates(_inputToStream);
                
                if(_inputToStream.InputAbilityTriggerZero)
                {
                    _playerAnimPipe.PerformDashAnim();
                }

                if (_inputToStream.InputAbilityTriggerOne)
                {
                    _playerAnimPipe.PerformAttackAnim();
                }
                
                if (_performHurtAnim)
                {
                    _playerAnimPipe.PerformHurtAnim();
                }

                if (_performDieAnim)
                {
                    _playerAnimPipe.PerformDieAnim();
                }
            }

            if (_hasNpcAnimations)
            {
                _npcAnimPipe.IsRunning = _enableNpcRun;

                if (_enableNpcAttackAnim)
                {
                    _npcAnimPipe.PerformAttackAnim();
                }

                if (_enableNpcAlertedAnim)
                {
                    _npcAnimPipe.PerformAlertedAnim();
                }

                if (_enableNpcKnockbackAnim)
                {
                    _npcAnimPipe.PerformKnockbackAnim();
                }

                if (_performDieAnim)
                {
                    _npcAnimPipe.PerformDieAnim();
                }
            }
        }
    }
}
