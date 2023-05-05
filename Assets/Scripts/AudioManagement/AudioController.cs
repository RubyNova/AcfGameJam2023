using AudioManagement.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Utilities;

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

        private class SoundEffectPlayInfo
        {
            public string EffectName { get; }
            public AudioSource Source { get; }

            public SoundEffectPlayInfo(string effectName, AudioSource source)
            {
                EffectName = effectName;
                Source = source;
            }
        }

        private const int AudioSourceCount = 2;

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

        private int _calmSourceToUseNextIndex;
        private int _cautionSourceToUseNextIndex;
        private int _dangerSourceToUseNextIndex;

        private AudioSource[] _calmSources;
        private AudioSource[] _cautionSources;
        private AudioSource[] _dangerSources;

        private Queue<Func<Coroutine>> _coroutinesToExecute;
        private Coroutine _currentRoutine;
        private MonoComponentPool<AudioSource> _soundEffectSources = null; // I removed the default constructor, Unity lifetime weirdness can eat shit
        private List<SoundEffectPlayInfo> _soundEffectPlayInfos;

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

            _calmMusicSourceZero.loop = true;
            _cautionMusicSourceZero.loop = true;
            _dangerMusicSourceZero.loop = true;
            _calmMusicSourceOne.loop = true;
            _cautionMusicSourceOne.loop = true;
            _dangerMusicSourceOne.loop = true;

            _calmMusicSourceZero.Stop();
            _cautionMusicSourceZero.Stop();
            _dangerMusicSourceZero.Stop();
            _calmMusicSourceOne.Stop();
            _cautionMusicSourceOne.Stop();
            _dangerMusicSourceOne.Stop();

            _calmSources = new AudioSource[]{ _calmMusicSourceZero, _calmMusicSourceOne };
            _cautionSources = new AudioSource[]{ _cautionMusicSourceZero, _cautionMusicSourceOne };
            _dangerSources = new AudioSource[]{ _dangerMusicSourceZero, _dangerMusicSourceOne };
            _coroutinesToExecute = new();
            _currentRoutine = null;
            _soundEffectSources = new(gameObject);
            _soundEffectPlayInfos = new();
        }

        private void Update()
        {
            ProcessCrossFadeQueue();
            ProcessSoundEffectPlayInfos();
        }

        private void ProcessCrossFadeQueue()
        {
            if (_currentRoutine != null || _coroutinesToExecute.Count == 0)
            {
                return;
            }

            _currentRoutine = _coroutinesToExecute.Dequeue()();
        }

        private void ProcessSoundEffectPlayInfos()
        {
            for (int i = _soundEffectPlayInfos.Count - 1; i >= 0; i--)
            {
                SoundEffectPlayInfo info = _soundEffectPlayInfos[i];

                if (!info.Source.isPlaying)
                {
                    _soundEffectPlayInfos.Remove(info);
                    _soundEffectSources.Return(info.Source);
                }
            }
        }

        private IEnumerator CrossFade(SoundtrackVariantState newState, AreaSoundtrackVariantData newData, bool terminate)
        {
            AudioClip clipToUseForTarget = null;
            AudioSource audioSourceForTarget = null;
            AudioSource audioSourceForCurrent = null;

            switch (_areaState)
            {
                case SoundtrackVariantState.Calm:
                    {
                        int index = _calmSourceToUseNextIndex - 1;

                        if (index < 0)
                        {
                            index = AudioSourceCount - 1;
                        }

                        audioSourceForCurrent = _calmSources[index];
                        break;
                    }
                case SoundtrackVariantState.Caution:
                    {
                        int index = _cautionSourceToUseNextIndex - 1;
                        
                        if (index < 0)
                        {
                            index = AudioSourceCount - 1;
                        }

                        audioSourceForCurrent = _cautionSources[index];
                        break;
                    }
                case SoundtrackVariantState.Danger:
                    {
                        int index = _dangerSourceToUseNextIndex - 1;
                        
                        if (index < 0)
                        {
                            index = AudioSourceCount - 1;
                        }

                        audioSourceForCurrent = _dangerSources[index];
                        break;
                    }
            }

            switch (newState)
            {
                case SoundtrackVariantState.Calm:
                    {
                        int index = _calmSourceToUseNextIndex++;

                        if (index >= AudioSourceCount) // redundant greater than but I like being safe
                        {
                            _calmSourceToUseNextIndex = 0;
                        }

                        clipToUseForTarget = newData != null ? newData.Calm : _areaData.Calm;
                        audioSourceForTarget = _calmSources[index];
                        break;
                    }
                case SoundtrackVariantState.Caution:
                    {
                        int index = _cautionSourceToUseNextIndex++;

                        if (index >= AudioSourceCount) // redundant greater than but I like being safe
                        {
                            _cautionSourceToUseNextIndex = 0;
                        }

                        clipToUseForTarget = newData != null ? newData.Caution : _areaData.Caution;
                        audioSourceForTarget = _cautionSources[index];
                        break;
                    }
                case SoundtrackVariantState.Danger:
                    {
                        int index = _dangerSourceToUseNextIndex++;

                        if (index >= AudioSourceCount) // redundant greater than but I like being safe
                        {
                            _dangerSourceToUseNextIndex = 0;
                        }

                        clipToUseForTarget = newData != null ? newData.Danger : _areaData.Danger;
                        audioSourceForTarget = _dangerSources[index];
                        break;
                    }
            }

            if (newData != null)
            {
                _areaData = newData;
            }

            if (terminate)
            {
                while (!Mathf.Approximately(audioSourceForCurrent.volume, 0))
                {
                    audioSourceForCurrent.volume -= _crossfadeSpeed * Time.deltaTime;
                    yield return null;
                }

                audioSourceForCurrent.volume = 0;
                audioSourceForCurrent.Stop();
                yield break;
            }
            
            audioSourceForTarget.clip = clipToUseForTarget;
            audioSourceForTarget.time = audioSourceForCurrent.time;
            audioSourceForTarget.Play();

            if (Mathf.Approximately(audioSourceForCurrent.volume, 0))
            {
                while(!Mathf.Approximately(audioSourceForTarget.volume, 1))
                {
                    audioSourceForTarget.volume += _crossfadeSpeed * Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                while(!Mathf.Approximately(audioSourceForTarget.volume, 1))
                {
                    audioSourceForTarget.volume += _crossfadeSpeed * Time.deltaTime;
                    audioSourceForCurrent.volume -= _crossfadeSpeed * Time.deltaTime;
                    yield return null;
                }
            }

            audioSourceForCurrent.volume = 0;
            audioSourceForTarget.volume = 1;
            audioSourceForCurrent.Stop();
            _currentRoutine = null;
        }

        public void EnqueueNewAreaMusic(AreaSoundtrackVariantData areaData)
        {
            _coroutinesToExecute.Enqueue(() => StartCoroutine(CrossFade(SoundtrackVariantState.Calm, areaData, false)));
        }

        public void EnqueueAreaStateChange(SoundtrackVariantState newState)
        {
            _coroutinesToExecute.Enqueue(() => StartCoroutine(CrossFade(newState, null, false)));
        }
    }
}
