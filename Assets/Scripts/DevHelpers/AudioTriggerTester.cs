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
        private EntitySoundBank _testBank;

        [SerializeField]
        private AudioController.SoundtrackVariantState _testState = AudioController.SoundtrackVariantState.Caution;

        private int _triggerCounter = 0;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.TryGetComponent<PlayerController>(out _))
            {
                return;
            }

            foreach(var effect in _testBank.SoundEffects)
            {
                _controller.PlaySoundEffect(effect.name, effect);
            }

            print(_triggerCounter);
            if (_triggerCounter == 0)
            {
                _controller.EnqueueNewAreaMusic(_variantData);
            }
            else
            {
                _controller.EnqueueAreaStateChange( _triggerCounter > 1 ? AudioController.SoundtrackVariantState.Calm : _testState);
            }

            _triggerCounter++;
        }
    }
}
