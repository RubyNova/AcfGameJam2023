using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Environment
{
    public class Exit : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private Room _owningRoom;

        [SerializeField]
        private Entrance _connectedEntrance;

        [Header("Configuration"), SerializeField]
        private bool _goesToAnotherArea;

        [SerializeField]
        private AsyncSceneSwitcher.SceneAsEnum _targetArea;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent<PlayerController>(out var controller))
            {
                return;
            }

            if (_connectedEntrance.OwningRoom.OwningAreaState != _owningRoom.OwningAreaState)
            {
                _owningRoom.OwningAreaState.IsActiveArea = false;
                _owningRoom.OwningAreaState.IsOnAlert = false;
            }

            _owningRoom.BecomeInactiveRoom();

            if (!_goesToAnotherArea)
            {
                _connectedEntrance.MovePlayerIntoRoom(controller);
            }
            else
            {
                AsyncSceneSwitcher.Instance.SwitchScene(_targetArea, x => x.allowSceneActivation = true);
            }
        }
    }
}
