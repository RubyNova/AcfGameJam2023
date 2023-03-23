using Movement;
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

        private Vector2 _inputData;
        private bool _shouldSprint;

        private void Awake()
        {
            _inputData = Vector2.zero;
            _shouldSprint = false;
        }

        // Update is called once per frame
        private void Update()
        {
            float finalMovementSpeed = _walkingSpeed;
            bool modifyInputSpeed = false;
            var finalInputData = _inputData;

            if (_shouldSprint && _inputData.y >= 0)
            {
                finalMovementSpeed = _runningSpeed;
            }
            else if (_inputData.y < -0.1f)
            {
                finalMovementSpeed = _crawlingSpeed;
                modifyInputSpeed = true;
            }

            if (modifyInputSpeed)
            {
                finalInputData.y = 0;
            }

            _mover.ApplyMove(finalInputData, finalMovementSpeed, _jumpForce);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _inputData = context.ReadValue<Vector2>();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            _shouldSprint = context.ReadValueAsButton();
        }
    }
}
