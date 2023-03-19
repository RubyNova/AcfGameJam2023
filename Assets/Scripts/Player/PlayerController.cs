using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField] 
        private Rigidbody2D _rigidbody;

        [Header("Configuration"), SerializeField]
        private float _walkingSpeed;

        [SerializeField]
        private float _runningSpeed;

        [SerializeField]
        private float _jumpForce;

        // Update is called once per frame
        private void Update()
        {
            Vector2 directionVector = new(Input.GetAxisRaw("Horizontal"), Input.GetKeyDown(KeyCode.Space) ? 1 : 0);
            directionVector.x *= Input.GetKey(KeyCode.LeftShift) ? _runningSpeed : _walkingSpeed;
            directionVector.y *= _jumpForce;
            
            if (Mathf.Approximately(directionVector.y, 0))
            {
                directionVector.y = _rigidbody.velocity.y;
            }

            _rigidbody.velocity = directionVector;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }
}
