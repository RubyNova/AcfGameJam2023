using AudioManagement;
using Environment;
using System.Collections;
using UnityEngine;

namespace SetPieceHelpers
{
    public class SoundEffectModification : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private AudioClip _soundEffect;

        [Header("Configuration"), SerializeField]
        private bool _loop;

        [SerializeField]
        private bool _isInfiniteLoop;

        [SerializeField]
        private float _loopDuration;

        private bool _isActive = false;
        private Coroutine _soundEffectRoutine = null;
        private string _soundEffectName = null;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _isActive = isActiveRoom;

            if (!_isActive)
            {
                StopEffect();
            }
        }

        public void PlayEffect()
        {
            if (!_isActive)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(_soundEffectName))
            {
                _soundEffectName = _soundEffect.name;
            }

            if (_loop && !_isInfiniteLoop)
            {
                _soundEffectRoutine = StartCoroutine(DoLoopDuration());
            }
            else
            {
                AudioController.Instance.PlaySoundEffect(_soundEffectName, _soundEffect, _loop);
            }

            IEnumerator DoLoopDuration()
            {
                AudioController.Instance.PlaySoundEffect(_soundEffectName, _soundEffect, _loop);
                yield return new WaitForSeconds(_loopDuration);
                AudioController.Instance.StopSoundEffect(_soundEffectName);
                _soundEffectRoutine = null;
            }
        }
        
        public void StopEffect()
        {
            if (string.IsNullOrWhiteSpace(_soundEffectName))
            {
                return;
            }

            if (_soundEffectRoutine != null)
            {
                StopCoroutine(_soundEffectRoutine);
                _soundEffectRoutine = null;
            }
            
            AudioController.Instance.StopSoundEffect(_soundEffectName);
        }
    }
}
