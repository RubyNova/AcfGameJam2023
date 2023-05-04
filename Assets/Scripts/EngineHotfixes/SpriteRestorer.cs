using System;
using UnityEngine;

namespace EngineHotfixes
{
    public class SpriteRestorer : MonoBehaviour
    {
        [Serializable]
        public class SpriteRendererDefaultConfig
        {
            [SerializeField]
            private SpriteRenderer _spriteRenderer;

            [SerializeField]
            private Sprite _defaultSprite;

            public void RestoreRendererToDefault()
            {
                _spriteRenderer.sprite = _defaultSprite;
            }
        }

        [Header("Configuration"), SerializeField]
        private SpriteRendererDefaultConfig[] _defaults;

        public void RestoreSpriteDefaults()
        {
            foreach (var config in _defaults)
            {
                config.RestoreRendererToDefault();
            }
        }
    }
}
