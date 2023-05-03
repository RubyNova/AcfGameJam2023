using System;
using System.Collections;
using UnityEngine;

namespace Environment
{
    public class AreaState : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private float _alertTimeout;

        public event Action<bool> ActiveAreaStateChanged;
        public event Action<bool> AlertStateChanged;

        private bool _isActiveArea;
        private bool _isOnAlert;

        private float _alertTimeRemaining;
        private Coroutine _alertRoutine;

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
    }
}
