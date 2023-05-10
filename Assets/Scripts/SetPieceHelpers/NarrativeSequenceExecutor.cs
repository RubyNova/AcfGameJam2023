using ACHNarrativeDriver;
using ACHNarrativeDriver.ScriptableObjects;
using Environment;
using UI;
using UnityEngine;

namespace SetPieceHelpers
{
    internal class NarrativeSequenceExecutor : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private NarrativeSequence _sequenceToExecute;

        private bool _isActive = false;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _isActive = isActiveRoom;
        }

        public void Execute()
        {
            if (!_isActive)
            {
                return;
            }

            MenuController.Instance.NarrativeMenu.ExecuteSequence(_sequenceToExecute);
        }
    }
}
