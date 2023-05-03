using UnityEngine;

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
        private Vector3 _squishModifierTop;
 
        [SerializeField]
        private Vector3 _squishModifierBot;
 
        [SerializeField]
        private Vector3 _squishModifierLeft;

        [SerializeField]
        private Vector3 _squishModifierRight;

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
            float angle = Vector2.Angle(directionAndLength, Vector2.right) * Mathf.Deg2Rad;

            var sinComponent = Mathf.Pow(_majorRadius, 2) * Mathf.Pow(Mathf.Sin(angle), 2);
            var cosComponent = Mathf.Pow(_minorRadius, 2) * Mathf.Pow(Mathf.Cos(angle), 2);
            var radius = (_majorRadius * _minorRadius) / Mathf.Sqrt(sinComponent + cosComponent);

            _eyeballIris.transform.localPosition = _irisLocalOrigin + (directionAndLength.normalized * radius);
        }
    }
}
