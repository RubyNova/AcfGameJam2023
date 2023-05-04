using System;
using UnityEditorInternal;
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

            public SpriteRendererDefaultConfig()
            { }

            public SpriteRendererDefaultConfig(SpriteRenderer spriteRenderer, Sprite defaultSprite)
            {
                _spriteRenderer = spriteRenderer;
                _defaultSprite = defaultSprite;
            }
        }

        [Header("Configuration"), SerializeField]
        private SpriteRendererDefaultConfig[] _defaults;

        [SerializeField]
        private AnimationState _animationState;

        public SpriteRendererDefaultConfig[] Defaults
        {
            get => _defaults;
            set => _defaults = value;
        }

        public void RestoreSpriteDefaults()
        {
            foreach (var config in _defaults)
            {
                config.RestoreRendererToDefault();
            }
        }
    }
}
