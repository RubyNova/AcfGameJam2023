using Player;
using System;
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
        private LayerMask _thrownLayer;

        [SerializeField]
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private DisposableItemConfig _config;

        private bool _isThrownByPlayer;
        private Vector2 _throwLocation;

        public bool IsThrownByPlayer => _isThrownByPlayer;
        public DisposableItemConfig Config => _config;

        public Vector2 ThrowLocation => _throwLocation;

        private void Awake()
        {
            _isThrownByPlayer = false;
            _throwLocation = Vector2.zero;
        }

        private void Start()
        {
            EnforceNewConfig(Config);
        }

        private IEnumerator MakeBangNoise()
        {
            _noiseCreator.AdjustNoiseRadius(Config.NoiseRadius);

            yield return new WaitForSeconds(Config.MaxRadiusDuration);

            _noiseCreator.AdjustNoiseRadius(0.0001f, Config.ReductionTime);

            if (_isThrownByPlayer)
            {
                yield return new WaitForSeconds(Config.TimeToDeletionPostThrow);
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
                var inventory = collision.gameObject.GetComponent<Inventory>();

                if (_config.IsHeavyItem)
                {
                    inventory.HeavyItems++;
                }
                else
                {
                    inventory.LightItems++;
                }

                if (component.HasItemEquipped)
                {
                    return;
                }

                ApplyPlayerParent(component);
            }
        }

        public void ApplyPlayerParent(PlayerController component)
        {
            _physicalCollider.enabled = false;
            _rigidbody.simulated = false;
            transform.SetParent(component.transform); // The sheer amount of debugging I had to do to make this work. I have no idea why it defaulted to this.gameObject when using collision.transform but HERE WE ARE *SCREAMS*
            transform.localPosition = Vector3.zero;
        }

        public void Throw(Vector2 direction, float force)
        {
            _throwLocation = transform.position;
            transform.SetParent(null);
            _isThrownByPlayer = true;
            gameObject.layer = _thrownLayer;
            _physicalCollider.enabled = true;
            _rigidbody.simulated = true;
            _rigidbody.velocity = direction * force;
        }

        internal void EnforceNewConfig(DisposableItemConfig item)
        {
            _config = item;
            _spriteRenderer.sprite = Config.ItemImage;
        }
    }
}
