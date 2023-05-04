using UnityEngine;
using UnityEngine.Serialization;

namespace Environment
{
    public class EyeballFollower : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private Transform _objectToFollow;

        [SerializeField]
        private Transform _eyeballIris;

        [Header("Configuration"), SerializeField]
        private float _minorRadius = +0.5f;
        
        [SerializeField]
        private float _majorRadius = +0.5f;

        [SerializeField]
        private Vector3 _squishModifierY;
  
        [SerializeField]
        private Vector3 _squishModifierX;

        private Vector2 _irisLocalOrigin;
        private Vector2 _irisGlobalOrigin;

        private void Awake()
        {
            _irisLocalOrigin = _eyeballIris.localPosition;
            _irisGlobalOrigin = _eyeballIris.position;
        }

        private void Update()
        {

            var directionAndLength = (Vector2)_objectToFollow.position - _irisGlobalOrigin;
            float angle = Vector2.Angle(directionAndLength, Vector2.right) * Mathf.Deg2Rad; // fuck my life this is so fucking stupid

            var sinComponent = Mathf.Pow(_majorRadius, 2) * Mathf.Pow(Mathf.Sin(angle), 2);
            var cosComponent = Mathf.Pow(_minorRadius, 2) * Mathf.Pow(Mathf.Cos(angle), 2);
            var radius = (_majorRadius * _minorRadius) / Mathf.Sqrt(sinComponent + cosComponent);

            var finalLocalPosition = Vector2.zero;

            if (Vector2.Distance(_objectToFollow.transform.position, _irisGlobalOrigin) >= radius)
            {
                finalLocalPosition = _irisLocalOrigin + (directionAndLength.normalized * radius);
            }
            else
            {
                finalLocalPosition = _irisLocalOrigin + directionAndLength;
            }
            
            _eyeballIris.transform.localPosition = finalLocalPosition;


            var left = _irisLocalOrigin.x - _majorRadius;
            var right = _irisLocalOrigin.x + _majorRadius;
            var top = _irisLocalOrigin.y + _minorRadius;
            var bot = _irisLocalOrigin.y - _minorRadius;

            var irisTranslation = finalLocalPosition - _irisLocalOrigin;

            var horizontalPercentage = Mathf.Abs(irisTranslation.x) / _majorRadius;
            var verticalPercentage = Mathf.Abs(irisTranslation.y) / _minorRadius;

            Vector3 finalLocalScale = new();

            finalLocalScale.x = Mathf.Lerp(_squishModifierX.x, 1f, verticalPercentage);
            finalLocalScale.y = Mathf.Lerp(_squishModifierY.y, 1f, horizontalPercentage);
            finalLocalScale.z = 1;

            _eyeballIris.transform.localScale = finalLocalScale;
        }
    }
}
