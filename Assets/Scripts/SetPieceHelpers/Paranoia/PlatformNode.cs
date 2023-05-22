using SetPieceHelpers.BossFightHelpers;
using UnityEngine;

namespace SetPieceHelpers.Paranoia
{
    public class PlatformNode : MonoBehaviour 
    {
        public enum JumpNodeDeterminationResult
        {
            Failed,
            RequiresRepositionToTheLeft,
            RequiresRepositionToTheRight,
            Success
        }

        [Header("Dependencies"), SerializeField]
        private string _playerTag = "Player";

        [SerializeField]
        private JumpNode _leftTarget;

        [SerializeField]
        private JumpNode _rightTarget;

        [SerializeField]
        private Transform _repositionLocationLeft;

        [SerializeField]
        private Transform _repositionLocationRight;
        
        [SerializeField]
        private PlatformNode[] _relatedPlatforms;

        public JumpNode LeftTarget => _leftTarget;
        public JumpNode RightTarget => _rightTarget;
        public Transform RepositionLocationLeft => _repositionLocationLeft;
        public Transform RepositionLocationRight => _repositionLocationRight;
        public PlatformNode[] RelatedPlatforms => _relatedPlatforms;

        public bool PlayerIsHere { get; private set; }

        public (JumpNodeDeterminationResult, JumpNode) DetermineCorrectJumpNodeBasedOnOrigin(Vector2 origin)
        {
            if (origin.x < _leftTarget.transform.position.x)
            {
                return (JumpNodeDeterminationResult.Success, _leftTarget);
            }
            
            if (origin.x > _rightTarget.transform.position.x)
            {
                return (JumpNodeDeterminationResult.Success, _rightTarget);
            }

            float distanceLeft = Vector2.Distance(origin, _leftTarget.transform.position);
            float distanceRight = Vector2.Distance(origin, _rightTarget.transform.position);

            JumpNodeDeterminationResult result = JumpNodeDeterminationResult.Failed;
            JumpNode returnNode = null;

            if (distanceLeft < distanceRight && _repositionLocationLeft != null)
            {
                result = JumpNodeDeterminationResult.RequiresRepositionToTheLeft;
                returnNode = LeftTarget;
            }
            else if (_repositionLocationRight != null)
            {
                result = JumpNodeDeterminationResult.RequiresRepositionToTheRight;
                returnNode = RightTarget;
            }

            return (result, returnNode);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_playerTag))
            {
                PlayerIsHere = true;
                return;
            }

            if (collision.gameObject.TryGetComponent<ParanoiaNPCCore>(out var core))
            {
                core.NotifyPlatformChanged(this);
                return;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(_playerTag))
            {
                PlayerIsHere = false;
                return;
            }

            if (collision.gameObject.TryGetComponent<ParanoiaNPCCore>(out var core))
            {
                core.NotifyPlatformChanged(null);
                return;
            }
        }
    }
}
