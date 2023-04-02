using Environment;
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

        [SerializeField]
        private AnimationDataPipe _pipe;

        [SerializeField]
        private Camera _targetCamera;

        [SerializeField]
        private Transform _aimTransform;

        [SerializeField]
        private LineRenderer _aimRenderer;

        [Header("Configuration"), SerializeField]
        private float _walkingSpeed;

        [SerializeField]
        private float _runningSpeed;

        [SerializeField]
        private float _crawlingSpeed;

        [SerializeField]
        private float _jumpForce;

        [SerializeField]
        private bool _hasAnimations;

        [SerializeField]
        private float _itemThrowForce;

        private InputInfo _inputInfo;
        private List<PlayerAbilityBehaviour> _abilities;
        private Dictionary<string, Coroutine> _activeAbilityCoroutines;
        private Vector2 _aimData;
        private bool _isMouse;
        private DisposableItem _item;

        public bool MovementIsOverridden { get; set; }

        private void Awake()
        {
            _inputInfo = new(Vector2.zero, false, false);
            _abilities = new();
            _activeAbilityCoroutines = new();
            MovementIsOverridden = false;
            _aimData = Vector2.zero;
            _isMouse = true;
        }

        private void Start()
        {
            _abilities.AddRange(gameObject.GetComponents<PlayerAbilityBehaviour>());
        }

        // Update is called once per frame
        private void Update()
        {
            PlayerContextObject context = new(this, _mover, _inputInfo, false, _activeAbilityCoroutines);

            UpdateAimUI();

            if (_hasAnimations)
            {
                _pipe.InputBasedUpdates(_inputInfo);
            }

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

            if (!MovementIsOverridden)
            {
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
            }


            if (Mathf.Approximately(_inputInfo.InputAxes.y, 1))
            {
                var copy = _inputInfo.InputAxes;
                copy.y = 0;
                _inputInfo.InputAxes = copy;
            }

            if (_inputInfo.InputAbilityTriggerZero)
            {
                _inputInfo.InputAbilityTriggerZero = false;
            }
        }

        private void UpdateAimUI()
        {
            if (_isMouse)
            {
                var diff = (Vector3)_aimData - _aimTransform.position;
                float angle = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) - 90;
                _aimTransform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                _aimTransform.up = _aimData;
            }

            _aimRenderer.SetPositions(new Vector3[] { _aimTransform.position, _aimTransform.position + (_aimTransform.up * 3) });
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
            if (!collision.gameObject.TryGetComponent<DisposableItem>(out var item))
            {
                return;
            }

            _item = item;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _inputInfo.InputAxes = context.ReadValue<Vector2>();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            _inputInfo.InputSprintModifier = context.ReadValueAsButton();
        }
        
        public void OnAbilityTriggerZero(InputAction.CallbackContext context)
        {
            _inputInfo.InputAbilityTriggerZero = context.ReadValueAsButton();
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            // TODO: I hate this SO much
            if (context.control.device is Gamepad)
            {
                _isMouse = false;
                _aimData = context.ReadValue<Vector2>();
            }
            else if (context.control.device is Mouse)
            {
                _isMouse = true;
                _aimData = _targetCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());
            }
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (!context.ReadValueAsButton() || _item == null)
            {
                return;
            }

            _item.Throw(_aimTransform.up, _itemThrowForce);
        }

        public void RegisterAbility<TAbility>() where TAbility : PlayerAbilityBehaviour
        {
            _abilities.Add(gameObject.AddComponent<TAbility>());
        }
    }
}
