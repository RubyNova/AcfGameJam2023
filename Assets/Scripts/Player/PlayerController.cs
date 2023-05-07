using Environment;
using Movement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    public class PlayerController : MonoSingleton<PlayerController>
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

        [SerializeField]
        private GameObject _itemPrefab;

        [SerializeField]
        private Inventory _inventory;

        [SerializeField]
        private Transform _spriteRoot;

        [SerializeField]
        private GameObject _crawlColliders;

        [SerializeField]
        private Collider2D _standingCollider;

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

        [SerializeField]
        private bool _allowMomentumBasedJumping = true;

        private InputInfo _inputInfo;
        private List<PlayerAbilityBehaviour> _abilities;
        private Dictionary<string, Coroutine> _activeAbilityCoroutines;
        private Vector2 _aimData;
        private bool _isMouse;
        private DisposableItem _item;
        private bool _heavyItemSelected;

        public bool HasItemEquipped => _item != null;

        public bool MovementIsOverridden { get; set; }

        protected override void OnInit()
        {
            _inputInfo = new(Vector2.zero, false, false, false, false);
            _abilities = new();
            _activeAbilityCoroutines = new();
            MovementIsOverridden = false;
            _aimData = Vector2.zero;
            _isMouse = true;
            _heavyItemSelected = false;
        }

        private void Start()
        {
            _abilities.AddRange(gameObject.GetComponents<PlayerAbilityBehaviour>());
            _crawlColliders.SetActive(false);
            _standingCollider.enabled = true;
        }

        // Update is called once per frame
        private void Update()
        {
            PlayerContextObject context = new(this, _mover, _inputInfo, false, _activeAbilityCoroutines, _pipe);

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
                var finalInputData = _inputInfo.InputAxes;
                bool shouldJump = _inputInfo.JumpInput;

                if (_inputInfo.InputSprintModifier && _inputInfo.InputAxes.y >= 0)
                {
                    finalMovementSpeed = _runningSpeed;
                }
                else if (_inputInfo.InputAxes.y < -0.1f)
                {
                    finalMovementSpeed = _crawlingSpeed;
                    shouldJump = false;
                    finalInputData.y = 0;
                    _crawlColliders.SetActive(true);
                    _standingCollider.enabled = false;
                }
                else
                {
                    _crawlColliders.SetActive(false);
                    _standingCollider.enabled = true;
                }

                if (_inputInfo.InputAxes.x > 0)
                {
                    _spriteRoot.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (_inputInfo.InputAxes.x < 0)
                {
                    _spriteRoot.localRotation = Quaternion.Euler(0, 180, 0);
                }

                bool forceSetVelocity = false;

                if (!_allowMomentumBasedJumping && ((_mover.IsGrounded && shouldJump) || !_mover.IsGrounded))
                {
                    finalMovementSpeed = _walkingSpeed;
                    forceSetVelocity = true;
                }

                _mover.ApplyMove(finalInputData, finalMovementSpeed, shouldJump, _jumpForce, context.ForceJump, forceSetVelocity);
            }
            else
            {
                    _crawlColliders.SetActive(false);
                    _standingCollider.enabled = true;
            }

            _inputInfo.JumpInput = false;

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

            if (item == null && ((item.Config.IsHeavyItem && _heavyItemSelected) || !(item.Config.IsHeavyItem && _heavyItemSelected)))
            {
                _item = item;
            }

            if (item.Config.IsHeavyItem)
            {
                _inventory.HeavyItems++;
            }
            else
            {
                _inventory.LightItems++;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        { 
            var input = context.ReadValue<Vector2>();
            
            if (Mathf.Approximately(input.y, 1))
            {
                input.y = 0;
            }

            _inputInfo.InputAxes = input;
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            _inputInfo.InputSprintModifier = context.ReadValueAsButton();
        }
        
        public void OnAbilityTriggerZero(InputAction.CallbackContext context)
        {
            _inputInfo.InputAbilityTriggerZero = context.ReadValueAsButton();
        }

        public void OnAbilityTriggerOne(InputAction.CallbackContext context)
        {
            _inputInfo.InputAbilityTriggerOne = context.ReadValueAsButton();
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

            if (_item.Config.IsHeavyItem)
            {
                _inventory.HeavyItems--;
                if (_inventory.HeavyItems > 0)
                {
                    var newItem = Instantiate(_itemPrefab).GetComponent<DisposableItem>();
                    newItem.ApplyPlayerParent(this);
                    newItem.EnforceNewConfig(_item.Config);
                    _item = newItem;
                }
                else
                {
                    _item = null;
                }
            }
            else
            {
                _inventory.LightItems--;
                if (_inventory.LightItems > 0)
                {
                    var newItem = Instantiate(_itemPrefab).GetComponent<DisposableItem>();
                    newItem.ApplyPlayerParent(this);
                    newItem.EnforceNewConfig(_item.Config);
                    _item = newItem;
                }
                else
                {
                    _item = null;
                }
            }

        }

        public void OnJump(InputAction.CallbackContext context)
        {
             _inputInfo.JumpInput = context.ReadValueAsButton();
        }

        public void RegisterAbility<TAbility>() where TAbility : PlayerAbilityBehaviour
        {
            _abilities.Add(gameObject.AddComponent<TAbility>());
        }

        public void EquipItem(DisposableItemConfig item)
        {
            if (item == null)
            {
                _item = Instantiate(_itemPrefab).GetComponent<DisposableItem>();
                _item.ApplyPlayerParent(this);
            }
            else
            {
                _item.EnforceNewConfig(item);
            }
        }
    }
}
