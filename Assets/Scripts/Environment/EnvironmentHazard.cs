using Player;
using UnityEngine;

namespace Environment
{
    public class EnvironmentHazard : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private string _playerTagName = "Player";

        [Header("Configuration"), SerializeField]
        private int _damageAmount = 10;

        [SerializeField]
        private float _damageCooldown = 0.7f;

        [SerializeField]
        private bool _useTriggerCollider = false;

        private float _damageCooldownRemaining = 0; // I hate that this is somehow more efficient than using Awake. Cringe.

        private void Update()
        {
            if (_damageCooldownRemaining <= 0)
            {
                return;
            }

            _damageCooldownRemaining -= Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_useTriggerCollider || !collision.gameObject.CompareTag(_playerTagName))
            {
                return;
            }

            collision.gameObject.GetComponent<PlayerHealthController>().AdjustHealth(_damageAmount);
            _damageCooldownRemaining = _damageCooldown;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_useTriggerCollider || !collision.gameObject.CompareTag(_playerTagName))
            {
                return;
            }

            collision.gameObject.GetComponent<PlayerHealthController>().AdjustHealth(_damageAmount);
            _damageCooldownRemaining = _damageCooldown;
        }
    }
}
