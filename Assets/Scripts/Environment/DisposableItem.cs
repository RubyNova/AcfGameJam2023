using Player;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Environment
{
    public class DisposableItem : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private NoiseCreator _noiseCreator;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Collider2D _physicalCollider;

        [SerializeField]
        private string _thrownLayerName;

        [SerializeField]
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private DisposableItemConfig _config;

        private bool _isThrownByPlayer;

        public bool IsThrownByPlayer => _isThrownByPlayer;

        private void Awake()
        {
            _isThrownByPlayer = false;
        }

        private void Start()
        {
            _spriteRenderer.sprite = _config.ItemImage;
        }

        private IEnumerator MakeBangNoise()
        {
            _noiseCreator.AdjustNoiseRadius(_config.NoiseRadius);

            yield return new WaitForSeconds(_config.MaxRadiusDuration);

            _noiseCreator.AdjustNoiseRadius(0.0001f, _config.ReductionTime);

            if (_isThrownByPlayer)
            {
                yield return new WaitForSeconds(_config.TimeToDeletionPostThrow);
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.TryGetComponent<PlayerController>(out var component))
            {
                StartCoroutine(MakeBangNoise());
            }
            else
            {
                _physicalCollider.enabled = false;
                _rigidbody.simulated = false;
                transform.SetParent(component.transform); // The sheer amount of debugging I had to do to make this work. I have no idea why it defaulted to this.gameObject when using collision.transform but HERE WE ARE *SCREAMS*
                transform.localPosition = Vector3.zero;
            }
        }

        public void Throw(Vector2 direction, float force)
        {
            transform.SetParent(null);
            _isThrownByPlayer = true;
            gameObject.layer = LayerMask.NameToLayer(_thrownLayerName);
            _physicalCollider.enabled = true;
            _rigidbody.simulated = true;
            _rigidbody.velocity = direction * force;
        }
    }
}
