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

        [SerializeField]
        private string _selfTagName;

        [Header("Configuration"), SerializeField, Range(0, 1)]
        private float _groundZeroToleranceValue = 1;

        private bool _isGrounded;
        private float _speedFromLastFrame;
        private Vector2 _directionForFrame;
        private float _horizontalSpeed;
        private float _jumpForce;
        private bool _forceJump;
        private bool _overrideMover;
        private float _gravityScale;
        private bool _inCollision;

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
            _inCollision = false;
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
            
            if (_inCollision && !_isGrounded && VerifyConflictingVelocityInput(directionVector, _rigidbody.velocity))
            {
                directionVector.x = 0;
            }

            directionVector.x *= finalSpeed;
            _speedFromLastFrame = finalSpeed;
            
            if (Mathf.Approximately(directionVector.y, 0))
            {
                directionVector.y = _rigidbody.velocity.y;
            }

            _rigidbody.velocity = directionVector;

            bool VerifyConflictingVelocityInput(Vector2 directionVector, Vector2 currentVelocity)
            {
                var entity = Physics2D.Raycast(transform.position, directionVector, directionVector.x, LayerMask.NameToLayer(_selfTagName));

                var directionToPosition = (entity.point - (Vector2)transform.position).normalized;
                
                if ((directionToPosition.x > 0 && directionVector.x > 0) || (directionToPosition.x < 0 && directionVector.x < 0))
                {
                    print($"conflict! Values: input dir: {directionVector.x} dirToPos: {directionToPosition.x}");
                    return true;
                }

                return false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _inCollision = true;
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

                if (Vector2.Dot(down, directionToPoint) >= _groundZeroToleranceValue)
                {
                    _rigidbody.velocity = Vector2.zero;
                    _isGrounded = true;
                    break;
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            _inCollision = false;
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
