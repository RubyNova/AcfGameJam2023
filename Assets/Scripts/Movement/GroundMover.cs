using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Movement
{
    public class GroundMover : MonoBehaviour
    {
        class CollisionDirectionDataBuffer
        {
            public int DataLength { get; set; } = 0;
            public Vector2[] CollisionDirections { get; set; } = new Vector2[20];
        }

        [Header("Dependencies"), SerializeField] 
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private string _groundTagName;

        [SerializeField]
        private string _selfTagName;

        [SerializeField]
        private Transform _positionToCheckColliderPointFrom;

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
        private bool _jumpInput;
        private Dictionary<GameObject, CollisionDirectionDataBuffer> _activeCollisionPointsPerGameObject;
        private List<Collider2D> _groundColliders;
        private bool _isJumping;
        private bool _rawVelocityApplied;

        public bool OverrideMover // lmao fuck
        {
            get => _overrideMover;
            set => _overrideMover = value;
        }

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
            _jumpInput = false;
            _activeCollisionPointsPerGameObject = new();
            _groundColliders = new();
            _isJumping = true;
            _rawVelocityApplied = false;
        }

        private void Start()
        {
            _gravityScale = _rigidbody.gravityScale;
        }

        private void FixedUpdate()
        {
            bool newIsGrounded = false;

            if (_isJumping)
            {
                _isGrounded = newIsGrounded;
                return;
            }

            for (int i = _groundColliders.Count - 1; i >= 0; i--)
            {
                Collider2D collider = _groundColliders[i];

                if (collider == null)
                {
                    _groundColliders.Remove(collider);
                    continue;
                }

                bool newState = IsGroundBelowWithinGivenTolerance(collider.ClosestPoint(_positionToCheckColliderPointFrom.position));

                if (newState)
                {
                    newIsGrounded = true;
                    break;
                }
            }

            _isGrounded = newIsGrounded;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_overrideMover)
            {
                return;
            }

            Vector2 directionVector = _directionForFrame;
            directionVector.y = _jumpInput ? 1 : directionVector.y;
            float finalSpeed = 0;

            if (_isGrounded)
            {
                finalSpeed = _horizontalSpeed;
                directionVector.y *= _jumpForce;

                if (directionVector.y > 0)
                {
                    _isJumping = true;
                }
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
                foreach(var (_, collisionData) in _activeCollisionPointsPerGameObject)
                {
                    for (int i = 0; i < collisionData.DataLength; i++)
                    {
                        Vector2 collisionDirection = collisionData.CollisionDirections[i];
                        
                        if ((collisionDirection.x > 0 && directionVector.x > 0) || (collisionDirection.x < 0 && directionVector.x < 0))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            _inCollision = true;    
            
            List<ContactPoint2D> contacts = new();
            _ = collision.GetContacts(contacts); // TODO: Unity APIs are questionable at best

            CollisionDirectionDataBuffer buffer = null;

            if (!_activeCollisionPointsPerGameObject.ContainsKey(collision.otherCollider.gameObject))
            {
                buffer = new CollisionDirectionDataBuffer();
                _activeCollisionPointsPerGameObject[collision.otherCollider.gameObject] = buffer;
            }
            else
            {
                buffer = _activeCollisionPointsPerGameObject[collision.otherCollider.gameObject];
            }

            buffer.DataLength = contacts.Count;

            for (int i = 0; i < contacts.Count; i++)
            {
                ContactPoint2D contact = contacts[i];
                buffer.CollisionDirections[i] = (contact.point - (Vector2)transform.position).normalized;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            _inCollision = false;

            if (_activeCollisionPointsPerGameObject.ContainsKey(collision.otherCollider.gameObject))
            {
                _activeCollisionPointsPerGameObject.Remove(collision.otherCollider.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.gameObject.CompareTag(_groundTagName) || _groundColliders.Contains(collider))
            {
                return;
            }

            _isJumping = false;
            _groundColliders.Add(collider);

            if (_rawVelocityApplied)
            {
                _rawVelocityApplied = false;
                _overrideMover = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (!collider.gameObject.CompareTag(_groundTagName) || !_groundColliders.Contains(collider))
            {
                return;
            }

            _groundColliders.Remove(collider);
        }

        private bool IsGroundBelowWithinGivenTolerance(Vector2 target)
        {
            var down = -transform.up;
            var directionToPoint = (target - (Vector2)transform.position).normalized;

            return Vector2.Dot(down, directionToPoint) >= _groundZeroToleranceValue;
        }

        public void ApplyMove(Vector2 directionInput, float horizontalSpeed, bool jumpInput, float jumpForce, bool forceJump = false, bool forceSetVelocity = false)
        {
            _overrideMover = false;
            _rigidbody.gravityScale = _gravityScale;
            _forceJump = forceJump;
            _directionForFrame = directionInput;
            _horizontalSpeed = horizontalSpeed;
            _jumpInput = jumpInput;
            _jumpForce = jumpForce;

            if (forceSetVelocity)
            {
                _speedFromLastFrame = horizontalSpeed;
            }
        }

        public void ApplyRawDirection(Vector2 directionAndSpeed)
        {
            _overrideMover = true;
            _rigidbody.gravityScale = 0;
            _rigidbody.velocity = directionAndSpeed;
        }

        public void ApplyRawVelocity(Vector2 velocity)
        {
            _overrideMover = true;
            _rigidbody.velocity = velocity;
            _rawVelocityApplied = true;
        }
    }
}
