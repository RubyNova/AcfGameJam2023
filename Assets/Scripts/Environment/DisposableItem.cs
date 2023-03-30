using System.Collections;
using UnityEngine;

namespace Environment
{
    public class DisposableItem : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private NoiseCreator _noiseCreator;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private DisposableItemConfig _config;

        private void Start()
        {
            _spriteRenderer.sprite = _config.ItemImage;
        }

        private IEnumerator MakeBangNoise()
        {
            _noiseCreator.AdjustNoiseRadius(_config.NoiseRadius);

            yield return new WaitForSeconds(_config.MaxRadiusDuration);

            _noiseCreator.AdjustNoiseRadius(0.0001f, _config.ReductionTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            StartCoroutine(MakeBangNoise());
        }
    }
}
