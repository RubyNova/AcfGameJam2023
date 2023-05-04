using System.Collections.Generic;
using UnityEngine;

namespace AudioManagement.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEntitySoundBank", menuName = "Audio Management / Entity Sound Bank")]
    public class EntitySoundBank : ScriptableObject
    {
        [SerializeField]
        private List<AudioClip> _soundEffects;

        public IReadOnlyList<AudioClip> SoundEffects => _soundEffects;
    }
}
