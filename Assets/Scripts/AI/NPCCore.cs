using Environment;
using Movement;
using Player;
using UnityEngine;

namespace AI
{
    public class NPCCore : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private GroundMover _mover;

        [SerializeField]
        private LayerMask _raycastConfig;

        [SerializeField]
        private string _playerTag;

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
        private float _immediateAlertVisionSubRange;

        [SerializeField]
        private Vector2 _maxAudioRange;

        [SerializeField]
        private float _suspicionGracePeriod;

        [SerializeField]
        private float _identificationTime;

        [SerializeField]
        private float _suspicionTime;

        [SerializeField]
        private float _proximityLimit;

        private BehaviourState _currentState;
        private PlayerIdentificationState _identificationState;
        private int _currentPatrolRouteIndex;
        private float _timeToNextPatrolMove;
        private bool _isCurrentlyMovingToPatrolPoint;
        private Vector2 _lastPointOfInterest;
        private float _suspicionTimeRemaining;
        private SuspicionSubState _suspicionSubState;
        private bool _visualSuspicionIsTracked;
        private bool _audioSuspicionIsTracked;
        private PlayerController _foundPlayerController;
        private Vector3 _roomStartPos;

        private void Awake()
        {
            SetUpInitialAIState();
        }

        private void Start()
        {
            _roomStartPos = transform.position;
        }

        private void SetUpInitialAIState()
        {
            _currentState = BehaviourState.IdleOrPatrolling;
            _identificationState = PlayerIdentificationState.Unaware;
            _currentPatrolRouteIndex = 0;
            _timeToNextPatrolMove = _idleTimePerPatrolPoint;
            _isCurrentlyMovingToPatrolPoint = true;
            _lastPointOfInterest = Vector2.zero;
            _suspicionTimeRemaining = 0;
            _suspicionSubState = SuspicionSubState.GracePeriod;
            _visualSuspicionIsTracked = false;
            _audioSuspicionIsTracked = false;
            _foundPlayerController = null;
            transform.position = _roomStartPos;
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
                case BehaviourState.Dead:
                    
                    break;
            }

            void HandlePatrolTick()
            {
                switch (_identificationState)
                {
                    case PlayerIdentificationState.Uncertain:
                        _suspicionTimeRemaining = _suspicionGracePeriod;
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

                MoveTowardsTarget(position, target);
            }

            void HandleSuspicionTick()
            {
                if (_identificationState == PlayerIdentificationState.Identified)
                {
                    _suspicionTimeRemaining = 0;
                    _currentState = BehaviourState.Chasing;
                    return;
                }

                bool waitTimeIsDone = Mathf.Approximately(_suspicionTimeRemaining, 0) || _suspicionTimeRemaining < 0;
                
                switch (_suspicionSubState)
                {
                    case SuspicionSubState.GracePeriod:
                        _mover.ApplyMove(new(0, 0), 0, 0); // stops the NPC
                        if (!waitTimeIsDone)
                        {
                            _suspicionTimeRemaining -= Time.deltaTime;
                            return;
                        }

                        if (!_visualSuspicionIsTracked && !_audioSuspicionIsTracked)
                        {
                            _currentState = BehaviourState.IdleOrPatrolling;
                            return;
                        }

                        _suspicionSubState = SuspicionSubState.Investigate;
                        _suspicionTimeRemaining = _suspicionTime;
                        break;

                    case SuspicionSubState.Investigate:
                        if (waitTimeIsDone)
                        {
                            _audioSuspicionIsTracked = false;
                            if (_visualSuspicionIsTracked)
                            {
                                _currentState = BehaviourState.Chasing;
                                return;
                            }

                            if (_audioSuspicionIsTracked)
                            {
                                _currentState = BehaviourState.Searching;
                                return;
                            }

                            _currentState = BehaviourState.IdleOrPatrolling;
                            return;
                        }
 
                        _suspicionTimeRemaining -= Time.deltaTime;

                        var position = transform.position;
                        var target = _lastPointOfInterest;
                        float distance = Vector2.Distance(position, target);

                        if (distance < 2)
                        {
                            _mover.ApplyMove(new(0, 0), 0, 0); // stops the NPC
                            return;
                        }

                        MoveTowardsTarget(position, target);
                        break;
                }
            }

            void HandleChasingTick()
            {
                var position = transform.position;
                var target = _lastPointOfInterest;

                if (_foundPlayerController != null && !_visualSuspicionIsTracked)
                {
                    var hitData = Physics2D.Raycast(position, (_foundPlayerController.transform.position - position).normalized, Mathf.Infinity, _raycastConfig);
                    if (hitData.collider != null)
                    {
                        if (hitData.transform == _foundPlayerController.transform)
                        {
                            _lastPointOfInterest = _foundPlayerController.transform.position;
                            target = _lastPointOfInterest;
                        }
                        else
                        {
                            _currentState = BehaviourState.Searching; // TODO: holy hell what is this duplicated code aaaaaaaa
                        }
                    }
                    else
                    {
                        _currentState = BehaviourState.Searching;
                    }
                }

                float distance = Vector2.Distance(position, target);

                if (distance < _proximityLimit)
                {
                    _mover.ApplyMove(new(0, 0), 0, 0); // stops the NPC
                    return;
                }

                MoveTowardsTarget(position, target, true);
            }

            void HandleSearchingTick()
            {
                HandleChasingTick();

                if (_visualSuspicionIsTracked)
                {
                    _currentState = BehaviourState.Chasing;
                }
            }

            void MoveTowardsTarget(Vector3 position, Vector2 target, bool run = false)
            {
                float horizontalValue = position.x - target.x;

                if ((horizontalValue > 0 && !Mathf.Approximately(transform.right.x, -1)) || (horizontalValue < 0 && !Mathf.Approximately(transform.right.x, 1)))
                {
                    transform.Rotate(0, 180, 0);
                }

                int walkingDirection = horizontalValue > 0 ? -1 : 1;

                _mover.ApplyMove(new(walkingDirection, 0), run ? _runningSpeed : _walkingSpeed, 0);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.TryGetComponent<DisposableItem>(out var itemComponent))
            {
                return;
            }

            _lastPointOfInterest = itemComponent.transform.position;

            if (_currentState == BehaviourState.IdleOrPatrolling)
            {
                _currentState = BehaviourState.Suspicious;
                _audioSuspicionIsTracked = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag(_playerTag))
            {
                return;
            }

            if (_foundPlayerController == null)
            {
                if (!collision.TryGetComponent<PlayerController>(out var finalComponent))
                {
                    finalComponent = collision.GetComponentInChildren<PlayerController>(true);
                }

                _foundPlayerController = finalComponent;
            }

            _visualSuspicionIsTracked = true;

            var playerPosition = collision.transform.position;
            var selfPosition = transform.position;
            var distance = Vector2.Distance(selfPosition, playerPosition);
            _lastPointOfInterest = playerPosition;

            switch (_currentState)
            {
                case BehaviourState.IdleOrPatrolling:
                case BehaviourState.Suspicious:
                    if (distance > _immediateAlertVisionSubRange)
                    {
                        _identificationState = PlayerIdentificationState.Uncertain;
                    }
                    else
                    {
                        _identificationState = PlayerIdentificationState.Identified;
                    }
                    break;
                case BehaviourState.Chasing:
                case BehaviourState.Searching:

                    break;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag(_playerTag))
            {
                return;
            }

            _visualSuspicionIsTracked = false;
        }
    }
}
