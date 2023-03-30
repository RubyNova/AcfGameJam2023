using System;
using System.Collections;
using UnityEngine;

namespace Environment
{
    public class NoiseCreator : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private CircleCollider2D _collider;

        private Coroutine _adjustmentRoutine;

        private void Awake()
        {
            _adjustmentRoutine = null;
        }

        private void ResetAdjustmentRoutine()
        {
            if (_adjustmentRoutine is null)
            {
                return;
            }

            StopCoroutine(_adjustmentRoutine);
            _adjustmentRoutine = null;
        }

        private IEnumerator AdjustRadiusOverTime(float radius, float interpolationTime)
        {
            float timeRemaining = interpolationTime;
            float originalRadius = _collider.radius;

            float difference = 0;
            bool isExpanding = true;

            if (radius > originalRadius)
            {
                difference = radius - originalRadius;
            }
            else if (originalRadius > radius)
            {
                difference = originalRadius - radius;
                isExpanding = false;
            }
            else
            {
                yield break;
            }

            float speed = difference / interpolationTime;

            if (isExpanding)
            {
                while (timeRemaining > 0)
                {
                    _collider.radius += speed * Time.deltaTime;
                    timeRemaining -= Time.deltaTime;
                    yield return null;
                }

                _collider.radius = radius;
            }
            else
            {
                while (timeRemaining > 0)
                {
                    _collider.radius -= speed * Time.deltaTime;
                    timeRemaining -= Time.deltaTime;
                    yield return null;
                }

                _collider.radius = radius;
            }

            _adjustmentRoutine = null;
        }

        public void AdjustNoiseRadius(float radius, float interpolationTime)
        {
            ResetAdjustmentRoutine();
            _adjustmentRoutine = StartCoroutine(AdjustRadiusOverTime(radius, interpolationTime));
        }

        public void AdjustNoiseRadius(float radius)
        {
            ResetAdjustmentRoutine();
            _collider.radius = radius;
        }
    }
}

