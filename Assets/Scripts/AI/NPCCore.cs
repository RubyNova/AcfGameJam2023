using Movement;
using UnityEngine;

namespace AI
{
    public class NPCCore : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private GroundMover _mover;

        [Header("Configuration"), SerializeField]
        private Vector2[] _patrolRoute;

        [SerializeField]
        private float _idleTimePerPatrolPoint;

        [SerializeField]
        private float _walkingSpeed;

        [SerializeField]
        private float _runningSpeed;

        [SerializeField]
        private float _jumpForce;

        [SerializeField]
        private Vector2 _maxVisionRange;

        [SerializeField]
        private Vector2 _immediateAlertVisionSubRange;

        [SerializeField]
        private Vector2 _maxAudioRange;

        private BehaviourState _currentState;
        private PlayerIdentificationState _identificationState;
        private int _currentPatrolRouteIndex;
        private float _timeToNextPatrolMove;
        private bool _isCurrentlyMovingToPatrolPoint;

        private void Awake()
        {
            _currentState = BehaviourState.IdleOrPatrolling;
            _identificationState = PlayerIdentificationState.Unaware;
            _currentPatrolRouteIndex = 0;
            _timeToNextPatrolMove = _idleTimePerPatrolPoint;
            _isCurrentlyMovingToPatrolPoint = true;
        }

        private void Update()
        {
            switch (_currentState)
            {
                case BehaviourState.IdleOrPatrolling:
                    HandlePatrolTick();
                    break;
                case BehaviourState.Suspicious:
                    HandleSuspicionTick();
                    break;
                case BehaviourState.Chasing:
                    HandleChasingTick();
                    break;
                case BehaviourState.Searching:
                    HandleSearchingTick();
                    break;
            }

            void HandlePatrolTick()
            {
                switch (_identificationState)
                {
                    case PlayerIdentificationState.Uncertain:
                        _currentState = BehaviourState.Suspicious;
                        return;
                    case PlayerIdentificationState.Identified:
                        _currentState = BehaviourState.Chasing;
                        return;
                }

                bool waitTimeIsDone = Mathf.Approximately(_timeToNextPatrolMove, 0) || _timeToNextPatrolMove < 0;

                if (!_isCurrentlyMovingToPatrolPoint && !waitTimeIsDone)
                {
                    _timeToNextPatrolMove -= Time.deltaTime;
                    return;
                }
              
                if (waitTimeIsDone)
                {
                    _isCurrentlyMovingToPatrolPoint = true;
                    _timeToNextPatrolMove = _idleTimePerPatrolPoint;
                    _currentPatrolRouteIndex++;

                    if (_currentPatrolRouteIndex >= _patrolRoute.Length)
                    {
                        _currentPatrolRouteIndex = 0;
                    }
                }
                 
                var position = transform.position;
                var target = _patrolRoute[_currentPatrolRouteIndex];
                float distance = Vector2.Distance(position, target);

                if (Mathf.Approximately(distance, 0) || distance < 0.2)
                {
                    _isCurrentlyMovingToPatrolPoint = false;
                    _mover.ApplyMove(new(0, 0), 0, 0); // stops the NPC
                    transform.position = target;
                    return;
                }

                float horizontalValue = position.x - target.x;
                int walkingDirection = horizontalValue > 0 ? -1 : 1;
                _mover.ApplyMove(new(walkingDirection, 0), _walkingSpeed, 0);
            }

            void HandleSuspicionTick()
            {

            }

            void HandleChasingTick()
            {

            }

            void HandleSearchingTick()
            {

            }
        }
    }
}
