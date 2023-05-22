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

        [SerializeField]
        private int _entranceExitId;

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

            _owningRoom.BecomeInactiveRoom();

            if (!_goesToAnotherArea)
            {
                _connectedEntrance.MovePlayerIntoRoom(controller);
            }
            else
            {
                _owningRoom.OwningAreaState.IsActiveArea = false;
                _owningRoom.OwningAreaState.IsOnAlert = false;
                AsyncSceneSwitcher.Instance.EntranceExitId = _entranceExitId;
                AsyncSceneSwitcher.Instance.SwitchScene(_targetArea, x => x.allowSceneActivation = true);
            }
        }
    }
}
