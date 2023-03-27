using Movement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private GroundMover _mover;

        [Header("Configuration"), SerializeField]
        private float _walkingSpeed;

        [SerializeField]
        private float _runningSpeed;

        [SerializeField]
        private float _crawlingSpeed;

        [SerializeField]
        private float _jumpForce;

        private InputInfo _inputInfo;
        private List<PlayerAbilityBehaviour> _abilities;
        private Dictionary<string, Coroutine> _activeAbilityCoroutines;

        private void Awake()
        {
            _inputInfo = new(Vector2.zero, false);
            _abilities = new();
            _activeAbilityCoroutines = new();
        }

        private void Start()
        {
            _abilities.AddRange(gameObject.GetComponents<PlayerAbilityBehaviour>());
        }

        // Update is called once per frame
        private void Update()
        {
            PlayerContextObject context = new(this, _mover, _inputInfo, false, false, _activeAbilityCoroutines);
            foreach (var ability in _abilities)
            {
                if (_activeAbilityCoroutines.ContainsKey(ability.Name))
                {
                    continue;
                }

                if (ability.TryExecute(context, out var abilityRoutine))
                {
                    _activeAbilityCoroutines.Add(ability.Name, abilityRoutine);
                }
            }

            float finalMovementSpeed = _walkingSpeed;
            bool modifyInputSpeed = false;
            var finalInputData = _inputInfo.InputAxes;

            if (_inputInfo.InputSprintModifier && _inputInfo.InputAxes.y >= 0)
            {
                finalMovementSpeed = _runningSpeed;
            }
            else if (_inputInfo.InputAxes.y < -0.1f)
            {
                finalMovementSpeed = _crawlingSpeed;
                modifyInputSpeed = true;
            }

            if (modifyInputSpeed)
            {
                finalInputData.y = 0;
            }

            _mover.ApplyMove(finalInputData, finalMovementSpeed, _jumpForce, context.ForceJump);

            if (Mathf.Approximately(_inputInfo.InputAxes.y, 1))
            {
                var copy = _inputInfo.InputAxes;
                copy.y = 0;
                _inputInfo.InputAxes = copy;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _inputInfo.InputAxes = context.ReadValue<Vector2>();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            _inputInfo.InputSprintModifier = context.ReadValueAsButton();
        }

        public void RegisterAbility<TAbility>() where TAbility : PlayerAbilityBehaviour
        {
            _abilities.Add(gameObject.AddComponent<TAbility>());
        }
    }
}
