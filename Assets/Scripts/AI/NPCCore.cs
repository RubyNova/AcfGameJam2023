using Environment;
using Movement;
using Player;
using System.Collections;
using UnityEngine;

namespace AI
{
    public class NPCCore : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private GroundMover _mover;

        [SerializeField]
        private LayerMask _raycastConfig;

        [SerializeField]
        private string _playerTag;

        [SerializeField]
        private NPCAnimationDataPipe _pipe;

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

        [SerializeField]
        private Vector2 _roomStartPos;

        [SerializeField]
        private float _searchTime;

        [SerializeField]
        private float _attackCooldown;

        [SerializeField]
        private int _attackDamage;

        [SerializeField]
        private bool _isFlyingMovement;

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
        private float _searchTimeLeft;
        private bool _hasBeenHitByItem;
        private Room _roomContext;
        private float _timeUntilAttackAvailable;

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
            _searchTimeLeft = 0;
            _hasBeenHitByItem = false;
            transform.SetPositionAndRotation(_roomStartPos, Quaternion.Euler(Vector3.zero));
            _timeUntilAttackAvailable = 0;

            if (_isFlyingMovement)
            {
                _mover.OverrideMover = true;
            }
        }

        private void Update()
        {
            if (_timeUntilAttackAvailable > 0)
            {
                _timeUntilAttackAvailable -= Time.deltaTime;
            }

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
                    StartCoroutine(PerformDie());
                    break;
            }

            void HandlePatrolTick()
            {
                if (_roomContext.OwningAreaState.IsOnAlert)
                {
                    _currentState = BehaviourState.Searching;
                    _lastPointOfInterest = _roomContext.LastReportedPlayerPosition;
                    return;
                }

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
                    StopNPCMoving();
                    transform.position = target;
                    return;
                }

                MoveTowardsTarget(position, target);
            }

            void HandleSuspicionTick()
            {
                if (_roomContext.OwningAreaState.IsOnAlert)
                {
                    _currentState = BehaviourState.Searching;
                    _lastPointOfInterest = _roomContext.LastReportedPlayerPosition;
                    return;
                }

                if (_identificationState == PlayerIdentificationState.Identified)
                {
                    _searchTimeLeft = _searchTime;
                    _suspicionTimeRemaining = 0;
                    _currentState = BehaviourState.Chasing;
                    _roomContext.OwningAreaState.IsOnAlert = true;
                    _pipe.PerformAlertedAnim();
                    return;
                }

                bool waitTimeIsDone = Mathf.Approximately(_suspicionTimeRemaining, 0) || _suspicionTimeRemaining < 0;

                switch (_suspicionSubState)
                {
                    case SuspicionSubState.GracePeriod:
                        _mover.ApplyMove(new(0, 0), 0, false, 0); // stops the NPC
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
                                _searchTimeLeft = _searchTime;
                                _currentState = BehaviourState.Chasing;
                                return;
                            }

                            if (_audioSuspicionIsTracked)
                            {
                                _searchTimeLeft = _searchTime;
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
                            StopNPCMoving();
                            return;
                        }

                        MoveTowardsTarget(position, target);
                        break;
                }
            }

            void HandleChasingTick(bool resetSearchTime = true)
            {
                if (resetSearchTime)
                {
                    _roomContext.OwningAreaState.IsOnAlert = true;
                    _roomContext.LastReportedPlayerPosition = _lastPointOfInterest;
                    _searchTimeLeft = _searchTime;
                }

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
                            _roomContext.LastReportedPlayerPosition = _lastPointOfInterest;
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
                    StopNPCMoving();
                    if (_timeUntilAttackAvailable <= 0)
                    {
                        _timeUntilAttackAvailable = _attackCooldown;
                        _foundPlayerController.GetComponent<PlayerHealthController>().AdjustHealth(_attackDamage);
                        _pipe.PerformAttackAnim();
                    }
                    return;
                }

                MoveTowardsTarget(position, target, true);
            }

            void HandleSearchingTick()
            {
                if (_searchTimeLeft <= 0)
                {
                    _hasBeenHitByItem = false;
                    _currentState = BehaviourState.IdleOrPatrolling;
                }

                HandleChasingTick(false);

                if (_visualSuspicionIsTracked)
                {
                    _currentState = BehaviourState.Chasing;
                }

                _searchTimeLeft -= Time.deltaTime;
            }

            void MoveTowardsTarget(Vector3 position, Vector2 target, bool run = false)
            {
                float horizontalValue = position.x - target.x;

                if ((horizontalValue > 0 && !Mathf.Approximately(transform.right.x, -1)) || (horizontalValue < 0 && !Mathf.Approximately(transform.right.x, 1)))
                {
                    transform.Rotate(0, 180, 0);

                }

                if (_isFlyingMovement)
                {
                    var direction = (target - (Vector2)position).normalized;
                    var speed = run ? _runningSpeed : _walkingSpeed;
                    _mover.ApplyRawDirection(direction * speed);
                }
                else
                {

                    int walkingDirection = horizontalValue > 0 ? -1 : 1;

                    _pipe.IsRunning = run;

                    _mover.ApplyMove(new(walkingDirection, 0), run ? _runningSpeed : _walkingSpeed, false, 0);
                }
            }


            IEnumerator PerformDie()
            {
                _pipe.PerformDieAnim();
                yield return new WaitForSeconds(0.3f); // hard coded magic based on animations ftw
                gameObject.SetActive(false);
            }
        }

        private void StopNPCMoving()
        {
            if (_isFlyingMovement)
            {
                _mover.ApplyRawDirection(Vector2.zero);
            }
            else
            {
                _mover.ApplyMove(new(0, 0), 0, false, 0); // stops the NPC
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.TryGetComponent<DisposableItem>(out var item) || !item.IsThrownByPlayer)
            {
                return;
            }

            if (item.Config.IsHeavyItem)
            {
                _currentState = BehaviourState.Dead;
            }
            else if (_currentState == BehaviourState.IdleOrPatrolling || _currentState == BehaviourState.Suspicious)
            {
                _currentState = BehaviourState.Searching;
                ConfigureItemHitFlags();
            }
            else
            {
                ConfigureItemHitFlags();
            }

            void ConfigureItemHitFlags()
            {
                _lastPointOfInterest = item.ThrowLocation;
                _hasBeenHitByItem = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.TryGetComponent<DisposableItem>(out var itemComponent) || !itemComponent.IsThrownByPlayer || _currentState == BehaviourState.Dead || _hasBeenHitByItem)
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
            if (!collision.CompareTag(_playerTag) || _currentState == BehaviourState.Dead)
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
                        _roomContext.LastReportedPlayerPosition = playerPosition;
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

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _roomContext = roomContext;

            if (!isActiveRoom)
            {
                gameObject.SetActive(false);
            }
            else
            {
                SetUpInitialAIState();

                if (roomContext.OwningAreaState.IsOnAlert)
                {
                    _currentState = BehaviourState.Searching;
                    _lastPointOfInterest = playerEntryPosition;
                }

                gameObject.SetActive(true);
            }
        }

        public void ForceDeath()
        {
            _currentState = BehaviourState.Dead;
        }
    }
}
