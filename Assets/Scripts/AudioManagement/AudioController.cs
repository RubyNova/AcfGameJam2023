using AudioManagement.ScriptableObjects;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Utilities;

namespace AudioManagement
{
    public class AudioController : MonoSingleton<AudioController> 
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

        [SerializeField]
        public GameObject _sfxAnchor;

        [Header("Configuration"), SerializeField]
        private float _crossfadeSpeed; 
        
        private AreaSoundtrackVariantData _areaData;
        private SoundtrackVariantState _areaState;

        private int _calmSourceToUseCurrentIndex;
        private int _cautionSourceToUseCurrentIndex;
        private int _dangerSourceToUseCurrentIndex;

        private AudioSource[] _calmSources;
        private AudioSource[] _cautionSources;
        private AudioSource[] _dangerSources;

        private Queue<Func<Coroutine>> _coroutinesToExecute;
        private Coroutine _currentRoutine;
        private MonoComponentPool<AudioSource> _soundEffectSources = null; // I removed the default constructor, Unity lifetime weirdness can eat shit
        private List<SoundEffectPlayInfo> _soundEffectPlayInfos;
        private bool _isMusicPlaying;
        protected override void OnInit()
        {
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
            _soundEffectSources = new(_sfxAnchor);
            _soundEffectPlayInfos = new();
            _isMusicPlaying = false;
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

            print(_currentRoutine == null);

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
                        int index = _calmSourceToUseCurrentIndex;

                        audioSourceForCurrent = _calmSources[index];
                        break;
                    }
                case SoundtrackVariantState.Caution:
                    {
                        int index = _cautionSourceToUseCurrentIndex;

                        audioSourceForCurrent = _cautionSources[index];
                        break;
                    }
                case SoundtrackVariantState.Danger:
                    {
                        int index = _dangerSourceToUseCurrentIndex;

                        audioSourceForCurrent = _dangerSources[index];
                        break;
                    }
            }

            switch (newState)
            {
                case SoundtrackVariantState.Calm:
                    {
                        int index = _areaState == SoundtrackVariantState.Calm && _isMusicPlaying ? ++_calmSourceToUseCurrentIndex : _calmSourceToUseCurrentIndex;

                        if (index >= AudioSourceCount) // redundant greater than but I like being safe
                        {
                            _calmSourceToUseCurrentIndex = 0;
                        }
                        print($"Index: {index}");

                        clipToUseForTarget = newData != null ? newData.Calm : _areaData.Calm;
                        audioSourceForTarget = _calmSources[index];
                        break;
                    }
                case SoundtrackVariantState.Caution:
                    {
                        int index = _areaState == SoundtrackVariantState.Caution && _isMusicPlaying ? ++_cautionSourceToUseCurrentIndex : _cautionSourceToUseCurrentIndex;

                        if (index >= AudioSourceCount) // redundant greater than but I like being safe
                        {
                            _cautionSourceToUseCurrentIndex = 0;
                        }
                        print($"Index: {index}");

                        clipToUseForTarget = newData != null ? newData.Caution : _areaData.Caution;
                        audioSourceForTarget = _cautionSources[index];
                        break;
                    }
                case SoundtrackVariantState.Danger:
                    {
                        int index = _areaState == SoundtrackVariantState.Danger && _isMusicPlaying ? ++_dangerSourceToUseCurrentIndex : _dangerSourceToUseCurrentIndex;

                        if (index >= AudioSourceCount) // redundant greater than but I like being safe
                        {
                            _dangerSourceToUseCurrentIndex = 0;
                        }
                        print($"Index: {index}");

                        clipToUseForTarget = newData != null ? newData.Danger : _areaData.Danger;
                        audioSourceForTarget = _dangerSources[index];
                        break;
                    }
            }

            if (newData != null)
            {
                _areaData = newData;
            }

            _areaState = newState;


            if (terminate)
            {
                while (!Mathf.Approximately(audioSourceForCurrent.volume, 0))
                {
                    audioSourceForCurrent.volume -= _crossfadeSpeed * Time.deltaTime;
                    yield return null;
                }

                audioSourceForCurrent.volume = 0;
                audioSourceForCurrent.Stop();
                _areaData = null;
                _areaState = SoundtrackVariantState.Calm;
                _isMusicPlaying = false;
                yield break;
            }
            
            _isMusicPlaying = true;
            
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

                audioSourceForCurrent.volume = 0;
                audioSourceForCurrent.Stop();
            }
            
            audioSourceForTarget.volume = 1;

            _currentRoutine = null;
        }

        public void EnqueueNewAreaMusic(AreaSoundtrackVariantData areaData, SoundtrackVariantState newState = SoundtrackVariantState.Calm)
        {
            _coroutinesToExecute.Enqueue(() => StartCoroutine(CrossFade(newState, areaData, false)));
        }

        public void EnqueueAreaStateChange(SoundtrackVariantState newState)
        {
            _coroutinesToExecute.Enqueue(() => StartCoroutine(CrossFade(newState, null, false)));
        }

        public void EnqueueStopMusic(bool immediateTerminate = false)
        {
            if (immediateTerminate)
            {
                _coroutinesToExecute.Enqueue(() => StartCoroutine(ImmediateTerminate()));

                IEnumerator ImmediateTerminate()
                {
                    switch (_areaState)
                    {
                        case SoundtrackVariantState.Calm:
                            _calmSources[_calmSourceToUseCurrentIndex].Stop();
                            _calmSourceToUseCurrentIndex = 0;
                            break;
                        case SoundtrackVariantState.Caution:
                            _cautionSources[_cautionSourceToUseCurrentIndex].Stop();
                            _cautionSourceToUseCurrentIndex = 0;
                            break;
                        case SoundtrackVariantState.Danger:
                            _dangerSources[_dangerSourceToUseCurrentIndex].Stop();
                            _dangerSourceToUseCurrentIndex = 0;
                            break;
                    }

                    _isMusicPlaying = false;
                    yield break;
                }
            }
            else
            {
                _coroutinesToExecute.Enqueue(() => StartCoroutine(CrossFade(SoundtrackVariantState.Calm, null, true)));
            }
        }


        public void PlaySoundEffect(string name, AudioClip effect, bool loop = false)
        {
            var source = _soundEffectSources.Borrow();
            source.Stop();
            source.clip = effect;
            source.loop = loop;
            source.time = 0;
            source.volume = 1;
            source.Play();
            _soundEffectPlayInfos.Add(new(name, source));
        }

        public void StopSoundEffect(string name)
        {
            for (int i = _soundEffectPlayInfos.Count - 1; i >= 0; i--)
            {
                var info = _soundEffectPlayInfos[i];

                if (info.EffectName == name)
                {
                    info.Source.Stop();
                    _soundEffectPlayInfos.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
