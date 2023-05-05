using AudioManagement;
using AudioManagement.ScriptableObjects;
using Player;
using UnityEngine;

namespace DevHelpers
{
    public class AudioTriggerTester : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        AudioController _controller;

        [SerializeField]
        private AreaSoundtrackVariantData _variantData;

        [SerializeField]
        private AudioController.SoundtrackVariantState _testState = AudioController.SoundtrackVariantState.Caution;

        private int _triggerCounter = 0;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.TryGetComponent<PlayerController>(out _))
            {
                return;
            }

            if (_triggerCounter == 0)
            {
                _controller.EnqueueNewAreaMusic(_variantData);
            }
            else
            {
                _controller.EnqueueAreaStateChange(_testState);
            }

            _triggerCounter++;
        }
    }
}
