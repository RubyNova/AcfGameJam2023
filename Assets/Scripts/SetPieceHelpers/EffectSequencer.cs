using Environment;
using System;
using System.Collections;
using UnityEngine;

namespace SetPieceHelpers
{
    public class EffectSequencer : RoomEntityBehaviour
    {
        [Serializable]
        public class EffectTimePair
        {
            [SerializeField]
            private EffectTrigger _effectTrigger;

            [SerializeField]
            private bool _waitForExplicitMoveNext;

            [SerializeField]
            private float _delayBeforeExecution;
            
            public void Deconstruct(out EffectTrigger trigger, out bool waitForExplcitMoveNext, out float delay)
            {
                trigger = _effectTrigger;
                waitForExplcitMoveNext = _waitForExplicitMoveNext;
                delay = _delayBeforeExecution;
            }
        }

        [Header("Dependencies"), SerializeField]
        private EffectTimePair[] _triggerInformation;

        private bool _isActive = false;
        private Coroutine _effectSequenceExecutionRoutine = null;
        private bool _moveNext = false;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _isActive = isActiveRoom;

            if (_isActive || _effectSequenceExecutionRoutine == null)
            {
                return;
            }

            StopCoroutine(_effectSequenceExecutionRoutine);
            _effectSequenceExecutionRoutine = null;
        }

        public void ExecuteSequence()
        {
            if (!_isActive)
            {
                return;
            }

            if (_effectSequenceExecutionRoutine != null)
            {
                throw new InvalidOperationException("Attempted to execute the same effect sequence while it was already executing. Are you unintentionally calling it twice in quick succession?");
            }

            _effectSequenceExecutionRoutine = StartCoroutine(RunEffectSequence());

            IEnumerator RunEffectSequence()
            {
                foreach (var (trigger, waitForMoveNext, delay) in _triggerInformation)
                {
                    float delayRemaining = delay;

                    if (waitForMoveNext)
                    {
                        while(!_moveNext)
                        {
                            yield return null;
                        }
                    }
                    else
                    {
                        while(delayRemaining > 0)
                        {
                            delayRemaining -= Time.deltaTime;
                            yield return null;
                        }
                    }

                    _moveNext = false;
                    trigger.Trigger();
                }
            }
        }

        public void MoveNext()
        {
            _moveNext = true;
        }
    }
}
