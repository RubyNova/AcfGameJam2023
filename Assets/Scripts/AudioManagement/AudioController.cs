using AudioManagement.ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace AudioManagement
{
    public class AudioController : MonoBehaviour
    {
        public enum SoundtrackVariantState
        {
            Calm,
            Caution,
            Danger
        }

        [Header("Dependencies"), SerializeField]
        public AudioSource _calmMusicSourceZero;

        [SerializeField]
        public AudioSource _calmMusicSourceOne;

        [SerializeField]
        public AudioSource _cautionMusicSourceZero;

        [SerializeField]
        public AudioSource _cautionMusicSourceOne;
        
        [SerializeField]
        public AudioSource _dangerMusicSourceZero;
        
        [SerializeField]
        public AudioSource _dangerMusicSourceOne;

        [Header("Configuration"), SerializeField]
        private float _crossfadeSpeed; 
        
        private AreaSoundtrackVariantData _areaData;
        private SoundtrackVariantState _areaState;

        private int _currentCalmSourceIndex;
        private int _currentCautionSourceIndex;
        private int _currentDangerSourceIndex;

        private AudioSource[] _calmSources;
        private AudioSource[] _cautionSources;
        private AudioSource[] _dangerSources;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _areaState = SoundtrackVariantState.Calm;
            _calmMusicSourceZero.volume = 0;
            _cautionMusicSourceZero.volume = 0;
            _dangerMusicSourceZero.volume = 0;
            _calmMusicSourceOne.volume = 0;
            _cautionMusicSourceOne.volume = 0;
            _dangerMusicSourceOne.volume = 0;

            _calmSources = new AudioSource[]{ _calmMusicSourceZero, _calmMusicSourceOne };
            _cautionSources = new AudioSource[]{ _calmMusicSourceZero, _calmMusicSourceOne };
            _dangerSources = new AudioSource[]{ _calmMusicSourceZero, _calmMusicSourceOne };

        }

        /*
        private IEnumerator CrossFade(SoundtrackVariantState newState, AreaSoundtrackVariantData newData = null)
        {
            AudioClip clipToUseForTarget = null;
            AudioSource audioSourceForTarget = null;
            AudioSource audioSourceForCurrent = null;
            switch (_areaState)
            {
                case SoundtrackVariantState.Calm:
                    audioSourceForCurrent = _calmMusicSource;
                    break;
                case SoundtrackVariantState.Caution:
                    audioSourceForCurrent = _cautionMusicSource;
                    break;
                case SoundtrackVariantState.Danger:
                    audioSourceForCurrent = _cautionMusicSource;
                    break;
            }

            switch (newState)
            {
                case SoundtrackVariantState.Calm:
                    clipToUseForTarget = newData != null ? newData.Calm : _areaData.Calm;
                    audioSourceForTarget = _calmMusicSource;
                    break;
                case SoundtrackVariantState.Caution:
                    clipToUseForTarget = newData != null ? newData.Caution : _areaData.Caution;
                    audioSourceForTarget = _cautionMusicSource;
                    break;
                case SoundtrackVariantState.Danger:
                    clipToUseForTarget = newData != null ? newData.Danger : _areaData.Danger;
                    audioSourceForTarget = _dangerMusicSource;
                    break;
            }

            if (_areaState == newState)
            {
                float curveTime = _crossfadeSpeed * 0.5; // slash it in half 
            }
            else
            {

            }
        }
        */

        public void PlayAreaMusic(AreaSoundtrackVariantData areaData)
        {

        }

        public void PlayEffect(AudioClip effect)
        {
        }

        public void StopMusic(bool fadeOut = true)
        {

        }
    }
}
