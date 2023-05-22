using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Environment
{
    public class AreaController : MonoBehaviour
    {
        [Serializable]
        public class AreaSwitchPair
        {
            [SerializeField]
            private AsyncSceneSwitcher.SceneAsEnum _linkedArea;

            [SerializeField]
            private Entrance _targetEntrance;

            public AsyncSceneSwitcher.SceneAsEnum LinkedArea => _linkedArea;
            public Entrance Entrance => _targetEntrance;

            public void Deconstruct(out AsyncSceneSwitcher.SceneAsEnum linkedArea, out Entrance targetEntrance)
            {
                linkedArea = _linkedArea;
                targetEntrance = _targetEntrance;
            }
        }

        [Header("Dependencies"), SerializeField]
        private float _alertTimeout;

        [SerializeField]
        private AsyncSceneSwitcher.SceneAsEnum _currentScene;

        [SerializeField]
        private AreaSwitchPair[] _startingEntrances;

        [SerializeField]
        private SaveHandler[] _saveRooms;

        public event Action<bool> ActiveAreaStateChanged;
        public event Action<bool> AlertStateChanged;

        private bool _isActiveArea;
        private bool _isOnAlert;
        private float _alertTimeRemaining;
        private Coroutine _alertRoutine;
        private bool _isInitialisedFromSave = false;

        public bool IsActiveArea
        { 
            get 
            { 
                return _isActiveArea;
            } 
            set
            {
                _isActiveArea = value;
                ActiveAreaStateChanged?.Invoke(_isActiveArea);
            }
        }

        public bool IsOnAlert
        { 
            get 
            { 
                return _isOnAlert;
            } 
            set
            {
                if (value)
                {
                    _alertTimeRemaining = _alertTimeout;
                    
                    if (_alertRoutine == null)
                    {
                        _alertRoutine = StartCoroutine(ExecuteAlertCooldown());
                    }
                }
                
                if (value == _isOnAlert)
                {
                    return;
                }

                _isOnAlert = value;
                AlertStateChanged?.Invoke(_isOnAlert);
                
                IEnumerator ExecuteAlertCooldown()
                {
                    while (_alertTimeRemaining > 0)
                    {
                        _alertTimeRemaining -= Time.deltaTime;
                        yield return null;
                    }

                    _isOnAlert = false;
                    _alertRoutine = null;
                }
            }
        }

        private void Awake()
        {
            _isActiveArea = false;
            _isOnAlert = false;
            _alertTimeRemaining = 0;
            _alertRoutine = null;
        }

        private void Start()
        {
            if (_isInitialisedFromSave)
            {
                return;
            }

            if (_startingEntrances.Length == 0)
            {
                throw new InvalidOperationException("Unable to move the player into the area correctly, as no starting entrances exist.");
            }

            if (AsyncSceneSwitcher.Instance.PreviousScene is null)
            {
                _startingEntrances[0].Entrance.MovePlayerIntoRoom(PlayerController.Instance);
            }
            else
            {
                var previousScene = AsyncSceneSwitcher.Instance.PreviousScene.Value;
                bool cameFromGameScene = AsyncSceneSwitcher.Instance.EntranceExitId != null;
                int entranceExitId = cameFromGameScene ? AsyncSceneSwitcher.Instance.EntranceExitId.Value : 0;

                foreach (var (linkedScene, entranceObject) in _startingEntrances)
                {

                    if (!cameFromGameScene)
                    {
                        entranceExitId = entranceObject.EntranceExitId; // give us the first entrance for this scene type
                    }

                    if (linkedScene == previousScene && entranceObject.EntranceExitId == entranceExitId)
                    {
                        entranceObject.MovePlayerIntoRoom(PlayerController.Instance);
                        return;
                    }
                }

                throw new InvalidOperationException("Unable to move the player into the area correctly, as no starting entrances are paired with the scene the player was previously in. Please double check your configuration.");
            }
        }

        public void InitialiseFromSave(int saveRoomIndex)
        {
            _isInitialisedFromSave = true;

        }
    }
}
