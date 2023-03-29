using System.Collections;
using UnityEngine;

namespace Environment
{
    public class DisposableItem : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private NoiseCreator _noiseCreator;

        [SerializeField]
        private float _noiseRadius;

        [SerializeField]
        private float _maxRadiusDuration;

        [SerializeField]
        private float _reductionTime;

        private IEnumerator MakeBangNoise()
        {
            _noiseCreator.AdjustNoiseRadius(_noiseRadius);

            yield return new WaitForSeconds(_maxRadiusDuration);

            _noiseCreator.AdjustNoiseRadius(0.0001f, _reductionTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            StartCoroutine(MakeBangNoise());
        }
    }
}
