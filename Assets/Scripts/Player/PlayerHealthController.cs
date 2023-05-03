using System;
using UnityEngine;

namespace Player
{
    public class PlayerHealthController : MonoBehaviour
    {
        public enum HealthUpdateKind
        {
            Additive,
            Subtractive,
            DirectSet
        }

        [Header("Dependencies"), SerializeField]
        private PlayerController _playerController;

        [SerializeField]
        private AnimationDataPipe _pipe;

        [Header("Configuration"), SerializeField]
        private bool _hasAnimations = false;

        [SerializeField]
        private int _startingHealth = 100;

        private int _currentHealth;

        public event Action<int> OnHealthChanged;

        public int CurrentHealth => _currentHealth;

        public void ResetPlayerToDefault(bool notifyEventSubscribers = false)
        {
            _currentHealth = _startingHealth;

            if (notifyEventSubscribers)
            {
                OnHealthChanged?.Invoke(_currentHealth);
            }
        }

        private void Awake()
        {
            ResetPlayerToDefault();
        }

        public void AdjustHealth(int amount, HealthUpdateKind updateKind = HealthUpdateKind.Subtractive)
        {
            switch (updateKind)
            {
                case HealthUpdateKind.Additive:
                    _currentHealth += amount;
                    break;
                case HealthUpdateKind.Subtractive:
                    if (_currentHealth == 0)
                    {
                        return;
                    }
                    _currentHealth -= amount;
                    break;
                case HealthUpdateKind.DirectSet:
                    _currentHealth = amount;
                    break;
            }

            if (_currentHealth < 0 )
            {
                _currentHealth = 0;
            }

            OnHealthChanged?.Invoke(_currentHealth);

            if (_currentHealth == 0)
            {
                _playerController.enabled = false;

                if (_hasAnimations)
                {
                    _pipe.PerformDieAnim();
                }    
            }
        }
    }
}
