using Environment;
using SetPieceHelpers.BossFightHelpers;
using UnityEngine;
using UnityEngine.Events;

namespace SetPieceHelpers.Paranoia
{
    public class HazardPlatformTracker : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private SpecificEntityWaitPhysicalCollision[] _platformCollisionCheckers;

        [SerializeField]
        private BossDefeatedTrigger _defeatedTrigger;

        private bool _active = false;
        private int _platformsDisabledCounter;
        private UnityAction _action;
        private bool _subscribed = false;
         

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _active = isActiveRoom;

            if (_action == null)
            {
                _action = new UnityAction(() => 
                {
                    _platformsDisabledCounter++;

                    if (_platformsDisabledCounter == _platformCollisionCheckers.Length)
                    {
                        _defeatedTrigger.EndBossBattle();
                    }
                });
            }

            if (_active && !_subscribed)
            {
                foreach (var checker in _platformCollisionCheckers)
                {
                    checker.TargetCollided.AddListener(_action);
                }
            }
            else if (_subscribed)
            {
                foreach (var checker in _platformCollisionCheckers)
                {
                    checker.TargetCollided.RemoveListener(_action);
                }
            }
        }
    }
}
