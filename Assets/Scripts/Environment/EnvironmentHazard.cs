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
        private bool _useTriggerCollider = false;

        [SerializeField]
        private Vector2[] _safetyTeleportLocations;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_useTriggerCollider || !collision.gameObject.CompareTag(_playerTagName))
            {
                return;
            }

            collision.gameObject.GetComponent<PlayerHealthController>().AdjustHealth(_damageAmount);
            ApplySafetyPosition(collision.gameObject.transform);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_useTriggerCollider || !collision.gameObject.CompareTag(_playerTagName))
            {
                return;
            }

            collision.gameObject.GetComponent<PlayerHealthController>().AdjustHealth(_damageAmount);
            ApplySafetyPosition(collision.gameObject.transform);
        }

        private void ApplySafetyPosition(Transform target)
        {
            var closestLocation = _safetyTeleportLocations[0];
            float currentDistanceTobeat = Vector2.Distance(closestLocation, target.position);

            for (int i = 1; i < _safetyTeleportLocations.Length; i++)
            {
                var testPosition = _safetyTeleportLocations[i];
                float testDistance = Vector2.Distance(testPosition, target.position);

                if (testDistance > currentDistanceTobeat)
                {
                    currentDistanceTobeat = testDistance;
                    closestLocation = testPosition;
                }
            }

            target.position = closestLocation;
        }
    }
}
