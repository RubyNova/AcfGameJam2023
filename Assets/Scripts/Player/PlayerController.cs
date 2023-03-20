using Movement;
using UnityEngine;

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
        private float _jumpForce;

        // Update is called once per frame
        private void Update()
        {
            _mover.ApplyMove(new(Input.GetAxisRaw("Horizontal"), (Input.GetKeyDown(KeyCode.Space) ? 1 : 0)), Input.GetKey(KeyCode.LeftShift) ? _runningSpeed : _walkingSpeed, _jumpForce);
        }
    }
}
