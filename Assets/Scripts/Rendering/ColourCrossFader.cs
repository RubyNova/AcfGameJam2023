using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Rendering
{
    public class ColourCrossFader : MonoBehaviour
    {
        public enum TargetType
        {
            Material,
            SpriteRenderer,
            UIImage
        };

        [field: SerializeField, HideInInspector]
        public TargetType ComponentType
        {
            get; set;
        }


        [field: SerializeField, HideInInspector]
        public Material[] Materials
        {
            get; set;
        }

        [field: SerializeField, HideInInspector]
        public SpriteRenderer[] SpriteRenderers
        {
            get; set;
        }

        [field: SerializeField, HideInInspector]
        public Image[] UIImages
        {
            get; set;
        }

        [SerializeField]
        private Color _colourNodeA;

        [SerializeField]
        private Color _colourNodeB;

        [SerializeField]
        private float _timeForCrossFade;

        private Coroutine _crossfadeRoutine;
        
        private Color _colourA;
        private Color _colourB;
        private Action<Color> _updateFuncValue;

        private void Awake()
        {
            _colourA = _colourNodeA;
            _colourB = _colourNodeB;
        }

        private void Start()
        {
            switch (ComponentType)
            {
                case TargetType.Material:
                    foreach (var material in Materials)
                    {
                        material.color = _colourNodeA;
                    }
                    break;
                case TargetType.SpriteRenderer:
                    foreach (var spriteRenderer in SpriteRenderers)
                    {
                        spriteRenderer.color = _colourNodeA;
                    }
                    break;
                case TargetType.UIImage:
                    foreach (var image in UIImages)
                    {
                        image.color = _colourNodeA;
                    }
                    break;
            }
        }

        private void SwapColours()
        {
            var startColourCopy = _colourA;
            _colourA = _colourB;
            _colourB = startColourCopy;
        }

        public void DoCrossFade()
        {
            UpdateColourUpdateLambda();

            _crossfadeRoutine = StartCoroutine(CrossFadeLogicInternal());

            IEnumerator CrossFadeLogicInternal()
            {
                float timePassed = 0;

                while (timePassed < _timeForCrossFade)
                {
                    if (Mathf.Approximately(_colourA.a, _colourB.a))
                    {
                        throw new Exception("FUCK");
                    }
                    _updateFuncValue(Color.Lerp(_colourA, _colourB, timePassed / _timeForCrossFade));
                    timePassed += Time.deltaTime;
                    yield return null;
                }

                _updateFuncValue(_colourB);
                SwapColours();
                _crossfadeRoutine = null;
            }
        }

        private void UpdateColourUpdateLambda()
        {
            switch (ComponentType)
            {
                case TargetType.Material:
                    _updateFuncValue = x => 
                    {
                        foreach (var material in Materials)
                        {
                            material.color = x;
                        }
                    };
                    break;
                case TargetType.SpriteRenderer:
                    _updateFuncValue = x => 
                    {
                        foreach (var spriteRenderer in SpriteRenderers)
                        {
                            spriteRenderer.color = x;
                        }
                    };
                    break;
                case TargetType.UIImage:
                    _updateFuncValue = x => 
                    {
                        foreach (var image in UIImages)
                        {
                            image.color = x;
                        }
                    };
                    break;
            }
        }

        public void JumpToEndOfCrossFade()
        {
            if (_crossfadeRoutine != null)
            {
                StopCoroutine(_crossfadeRoutine);
                _crossfadeRoutine = null;
            }

            if (_updateFuncValue == null)
            {
               UpdateColourUpdateLambda();
            }

            _updateFuncValue(_colourB);
            SwapColours();
        }
    }

}
