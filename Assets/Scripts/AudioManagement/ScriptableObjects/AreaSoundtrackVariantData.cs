using UnityEngine;

namespace AudioManagement.ScriptableObjects
{

    [CreateAssetMenu(fileName = "NewAreaSoundtrackVariantData", menuName = "Audio Management / Area Soundtrack Variant Data")]
    public class AreaSoundtrackVariantData : ScriptableObject
    {
        [Header("Dependencies"), SerializeField]
        private AudioClip _calm;

        [SerializeField]
        private AudioClip _caution;

        [SerializeField]
        private AudioClip _danger;

        [SerializeField]
        private bool _treatCalmAsSoloVariant;

        public bool TreatCalmAsSoloVariant => _treatCalmAsSoloVariant;
        public AudioClip Calm => _calm;
        public AudioClip Caution => _caution;
        public AudioClip Danger => _danger;
    }
}
