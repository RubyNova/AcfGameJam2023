using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Movement
{
    public class GroundMover : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField] 
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private string _groundTagName;

        [Header("Configuration"), SerializeField]
        private float _groundZeroToleranceValue = 1;

        private bool _isGrounded;
        private float _speedFromLastFrame;
        private Vector2 _directionForFrame;
        private float _horizontalSpeed;
        private float _jumpForce;
        private bool _forceJump;
        private bool _overrideMover;
        private float _gravityScale;

        public bool IsGrounded => _isGrounded;

        private void Awake()
        {
            _isGrounded = false;
            _speedFromLastFrame = 0;
            _directionForFrame = Vector2.zero;
            _horizontalSpeed = 0;
            _jumpForce = 0;
            _forceJump = false;
            _overrideMover = false;
        }

        private void Start()
        {
            _gravityScale = _rigidbody.gravityScale;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_overrideMover)
            {
                return;
            }

            Vector2 directionVector = _directionForFrame;
            float finalSpeed = 0;

            if (_isGrounded)
            {
                finalSpeed = _horizontalSpeed;
                directionVector.y *= _jumpForce;
            }
            else
            {
                finalSpeed = _speedFromLastFrame;

                if (_forceJump)
                {
                    directionVector.y *= _jumpForce;
                }
                else
                {
                    directionVector.y = 0;
                }
            }

            directionVector.x *= finalSpeed;
            _speedFromLastFrame = finalSpeed;
            
            if (Mathf.Approximately(directionVector.y, 0))
            {
                directionVector.y = _rigidbody.velocity.y;
            }

            _rigidbody.velocity = directionVector;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag(_groundTagName))
            {
                return;
            }

            List<ContactPoint2D> contacts = new();
            _ = collision.GetContacts(contacts); // TODO: Unity APIs are questionable at best

            foreach (ContactPoint2D contact in contacts)
            {
                var down = -transform.up;
                var directionToPoint = (contact.point - (Vector2)transform.position).normalized;
                var dot = Vector2.Dot(down, directionToPoint);

                print(dot);
                if (dot >= _groundZeroToleranceValue)
                {
                    print("STOP MOVING");
                    _rigidbody.velocity = Vector2.zero;
                    _isGrounded = true;
                    break;
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag(_groundTagName))
            {
                return;
            }

            _isGrounded = false;
        }

        public void ApplyMove(Vector2 directionInput, float horizontalSpeed, float jumpForce, bool forceJump = false)
        {
            _overrideMover = false;
            _rigidbody.gravityScale = _gravityScale;
            _forceJump = forceJump;
            _directionForFrame = directionInput;
            _horizontalSpeed = horizontalSpeed;
            _jumpForce = jumpForce;
        }

        public void ApplyRawDirection(Vector2 directionAndSpeed)
        {
            _overrideMover = true;
            _rigidbody.gravityScale = 0;
            _rigidbody.velocity = directionAndSpeed;
        }
    }
}
