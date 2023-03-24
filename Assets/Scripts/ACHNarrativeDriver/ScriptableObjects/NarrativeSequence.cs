using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ACHNarrativeDriver.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewNarrativeSequence", menuName = "ACH Narrative Driver / NarrativeSequence")]
    public class NarrativeSequence : ScriptableObject
    {
        [Serializable]
        public class CharacterDialogueInfo
        {
            [SerializeField] private Character _character;
            [SerializeField] private bool _hasPoseIndex;
            [SerializeField] private int _poseIndex;
            [SerializeField] private bool _hasPlayMusicIndex;
            [SerializeField] private int _playMusicIndex;
            [SerializeField] private bool _hasPlaySoundEffectIndex;
            [SerializeField] private int _playSoundEffectIndex;
            [SerializeField] private bool _hasExecuteCustomEffectIndex;
            [SerializeField] private int _executeCustomEffectIndex;
            [SerializeField] private string _text;

            public Character Character
            {
                get => _character;
                set => _character = value;
            }

            public int? PoseIndex
            {
                get => _hasPoseIndex ? _poseIndex : null;
                set
                {
                    if (value.HasValue)
                    {
                        _poseIndex = value.Value;
                        _hasPoseIndex = true;
                    }
                    else
                    {
                        _hasPoseIndex = false;
                    }
                }
            }
            
            public int? PlayMusicIndex
            {
                get => _hasPlayMusicIndex ? _playMusicIndex : null;
                set
                {
                    if (value.HasValue)
                    {
                        _playMusicIndex = value.Value;
                        _hasPlayMusicIndex = true;
                    }
                    else
                    {
                        _hasPlayMusicIndex = false;
                    }
                }
            }
            
            public int? PlaySoundEffectIndex
            {
                get => _hasPlaySoundEffectIndex ? _playSoundEffectIndex : null;
                set
                {
                    if (value.HasValue)
                    {
                        _playSoundEffectIndex = value.Value;
                        _hasPlaySoundEffectIndex = true;
                    }
                    else
                    {
                        _hasPlaySoundEffectIndex = false;
                    }
                }
            }
            
            public int? ExecuteCustomEffectIndex
            {
                get => _hasExecuteCustomEffectIndex ? _executeCustomEffectIndex : null;
                set
                {
                    if (value.HasValue)
                    {
                        _executeCustomEffectIndex = value.Value;
                        _hasExecuteCustomEffectIndex = true;
                    }
                    else
                    {
                        _hasExecuteCustomEffectIndex = false;
                    }
                }
            }

            public string Text
            {
                get => _text;
                set => _text = value;
            }

            public override string ToString()
            {
                return $"Character: {Character.Name}, HasPoseIndex: {_hasPoseIndex}, {(_hasPoseIndex ? "PoseIndex: " + PoseIndex + ", " : string.Empty)}Text: {Text}";
            }
        }

        [Serializable]
        public class ChoiceInfo
        {
            [SerializeField] private string _choiceText;
            [SerializeField] private NarrativeSequence _narrativeResponse;

            public string ChoiceText
            {
                get => _choiceText;
                set => _choiceText = value;
            }

            public NarrativeSequence NarrativeResponse
            {
                get => _narrativeResponse;
                set => _narrativeResponse = value;
            }
        }
        
        [SerializeField] private NarrativeSequence _nextSequence;
        [SerializeField] private List<AudioClip> _musicFiles;
        [SerializeField] private List<AudioClip> _soundEffectFiles;
        [SerializeField] private List<GameObject> _customEffectPrefabs;
        [SerializeField] private List<CharacterDialogueInfo> _characterDialoguePairs;
        [SerializeField] private List<ChoiceInfo> _choices;

        public List<ChoiceInfo> Choices
        {
            get => _choices;
            set => _choices = value;
        }

        public NarrativeSequence NextSequence
        {
            get => _nextSequence;
            set => _nextSequence = value;
        }

        public List<AudioClip> MusicFiles
        {
            get => _musicFiles;
            set => _musicFiles = value;
        }
        
        public List<AudioClip> SoundEffectFiles
        {
            get => _soundEffectFiles;
            set => _soundEffectFiles = value;
        }

        public List<GameObject> CustomEffectPrefabs 
        {
            get => _customEffectPrefabs;
            set => _customEffectPrefabs = value;
        }

        public List<CharacterDialogueInfo> CharacterDialoguePairs
        {
            get => _characterDialoguePairs;
            set => _characterDialoguePairs = value;
        }

        [field: SerializeField, HideInInspector]
        public string SourceScript { get; set; }
    }
}
