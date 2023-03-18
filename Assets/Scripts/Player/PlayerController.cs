using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Rigidbody2D _rigidbody;

        [Header("Configuration")]
        [SerializeField] private float _speed;

        private Vector2 _rawInput;

        private void FixedUpdate()
        {
            var moveAmountRaw = _rawInput * _speed;
            var moveAmount = moveAmountRaw * Time.deltaTime;

            _rigidbody.velocity = moveAmount;
        }

        // Update is called once per frame
        private void Update()
        {
            _rawInput.x = Input.GetAxisRaw("Horizontal");
        }
    }
}
