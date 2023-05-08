using ACHNarrativeDriver;
using ACHNarrativeDriver.ScriptableObjects;
using UnityEngine;

namespace DevHelpers
{
    public class NarrativeSequenceUITester : MonoBehaviour
    {
        [SerializeField] private NarrativeUIController _target;
        [SerializeField] private NarrativeSequence _targetSequence;

        private void Start()
        {
            _target.ExecuteSequence(_targetSequence);
        }
    }
}