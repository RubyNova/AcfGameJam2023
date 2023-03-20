using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class GroundMover : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField] 
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private string _groundTagName;

        private bool _isGrounded;
        private float _speedFromLastFrame;
        private Vector2 _directionForFrame;
        private float _horizontalSpeed;
        private float _jumpForce;

        // Update is called once per frame
        private void Update()
        {
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
                directionVector.y = 0;
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
            if (collision.gameObject.tag != _groundTagName)
            {
                return;
            }

            _rigidbody.velocity = Vector2.zero;
            _isGrounded = true;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag != _groundTagName)
            {
                return;
            }

            _isGrounded = false;
        }

        public void ApplyMove(Vector2 directionInput, float horizontalSpeed, float jumpForce)
        {
            _directionForFrame = directionInput;
            _horizontalSpeed = horizontalSpeed;
            _jumpForce = jumpForce;
        }
    }
}
