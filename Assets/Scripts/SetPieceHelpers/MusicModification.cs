using AudioManagement;
using AudioManagement.ScriptableObjects;
using Environment;
using System;
using UnityEngine;

namespace SetPieceHelpers
{
    public class MusicModification : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private AreaSoundtrackVariantData _soundtrackData;

        [Header("Configuration"), SerializeField]
        private AudioController.SoundtrackVariantState _soundtrackVariantStateToApply;

        [SerializeField]
        private bool _isTerminationInstruction;

        [SerializeField]
        private bool _shouldimmediatelyTerminate;

        private bool _isActive = false;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _isActive = isActiveRoom;
        }

        public void EnqueueAudioChanges()
        {
            if (!_isActive)
            {
                return;
            }

            if (_isTerminationInstruction)
            {
                AudioController.Instance.EnqueueStopMusic(_shouldimmediatelyTerminate);
            }
            else
            {
                AudioController.Instance.EnqueueNewAreaMusic(_soundtrackData, _soundtrackVariantStateToApply);
            }
        }
    }
}
