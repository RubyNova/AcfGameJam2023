using System.Collections;
using System.Text;
using ACHNarrativeDriver.Api;
using ACHNarrativeDriver.ScriptableObjects;
using AudioManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ACHNarrativeDriver
{
    public class NarrativeUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _narrativeTextBox;
        [SerializeField] private GameObject[] _dialogueBoxes;
        [SerializeField] private GameObject _dialoguePanel;
        public UnityEvent sequenceFinishedEvent;

        private Coroutine _rollingTextRoutine;
        private readonly WaitForSeconds _rollingCharacterTime = new(0.04f);
        private NarrativeSequence _currentNarrativeSequence;
        private bool _isCurrentlyExecuting;
        private bool _nextDialogueLineRequested;
        private int _currentDialogueIndex;
        private Interpreter _narrativeInterpreter;
        private RuntimeVariables _narrativeRuntimeVariables;
        private bool _hasFocus;
        
        public NarrativeSequence LastPlayedSequence { get; private set; }
        
        private void Awake()
        {
            _isCurrentlyExecuting = false;
            _currentDialogueIndex = 0;
            _narrativeInterpreter = new();
            _narrativeRuntimeVariables = FindObjectOfType<RuntimeVariables>();
            _hasFocus = false;
            _dialoguePanel.SetActive(false);
        }

        private void Update()
        {
            if (!_isCurrentlyExecuting || !_nextDialogueLineRequested)
            {
                return;
            }

            if (_currentDialogueIndex >= _currentNarrativeSequence.CharacterDialoguePairs.Count && _rollingTextRoutine is null)
            {
                _currentDialogueIndex = 0;

                if (_currentNarrativeSequence.NextSequence is null)
                {
                    _hasFocus = false;
                    LastPlayedSequence = _currentNarrativeSequence;
                    sequenceFinishedEvent.Invoke();
                    _dialogueBoxes[LastPlayedSequence.DialogueBoxRenderIndex].SetActive(false);
                    _dialoguePanel.SetActive(false);
                    _isCurrentlyExecuting = false;
                    _currentNarrativeSequence = null;
                    return;
                }

                _currentNarrativeSequence = _currentNarrativeSequence.NextSequence;
            }

            _nextDialogueLineRequested = false;

            if (_rollingTextRoutine is not null)
            {
                ResetRollingTextRoutine();
                _narrativeTextBox.text =
                    _currentNarrativeSequence.CharacterDialoguePairs[_currentDialogueIndex - 1].Text;
                return;
            }

            var characterDialogueInfo = _currentNarrativeSequence.CharacterDialoguePairs[_currentDialogueIndex];

            _rollingTextRoutine =
                StartCoroutine(
                    PerformRollingText(characterDialogueInfo));
            _currentDialogueIndex++;
        }
        
        private void ResetRollingTextRoutine()
        {
            StopCoroutine(_rollingTextRoutine);
            _rollingTextRoutine = null;
        }

        private IEnumerator PerformRollingText(NarrativeSequence.CharacterDialogueInfo targetDialogueInfo)
        {
            StringBuilder sb = new();
            var resolvedText = _narrativeInterpreter.ResolveRuntimeVariables(targetDialogueInfo.Text, _narrativeRuntimeVariables != null ? _narrativeRuntimeVariables.ReadOnlyVariableView : null);

            foreach (var character in resolvedText)
            {
                sb.Append(character);
                _narrativeTextBox.text = sb.ToString();
                yield return _rollingCharacterTime;
            }

            _rollingTextRoutine = null;
        }

        public void ExecuteSequence(NarrativeSequence targetSequence)
        {
            if (_rollingTextRoutine is not null)
            {
                ResetRollingTextRoutine();
            }
            
            _dialoguePanel.SetActive(true);
            _hasFocus = true;
            _narrativeTextBox.text = string.Empty;
            _currentNarrativeSequence = targetSequence;
            _nextDialogueLineRequested = true;
            _isCurrentlyExecuting = true;
            _dialogueBoxes[targetSequence.DialogueBoxRenderIndex].SetActive(true);
        }

        public void ExecuteNextDialogueLine()
        {
            if (_currentNarrativeSequence is null)
            {
                return;
            }

            _nextDialogueLineRequested = true;
        }

        public void OnInteractTrigger()
        {
            if (!_hasFocus)
            {
                return;
            }
            
            ExecuteNextDialogueLine();
        }
    }
}