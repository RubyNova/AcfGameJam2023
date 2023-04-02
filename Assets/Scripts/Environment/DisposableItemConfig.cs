using UnityEngine;

namespace Environment
{
    [CreateAssetMenu(fileName = "NewItemConfig", menuName = "Dream Environment / Disposable Item")]
    public class DisposableItemConfig : ScriptableObject
    {
        [SerializeField]
        private float _noiseRadius;

        [SerializeField]
        private float _maxRadiusDuration;

        [SerializeField]
        private float _reductionTime;

        [SerializeField]
        private Sprite _itemImage;

        [SerializeField]
        private float _timeToDeletionPostThrow;

        [SerializeField]
        private float _itemWeight;

        public float NoiseRadius => _noiseRadius;
        public float MaxRadiusDuration => _maxRadiusDuration;
        public float ReductionTime => _reductionTime;
        public Sprite ItemImage => _itemImage;
        public float TimeToDeletionPostThrow => _timeToDeletionPostThrow;
        public float ItemWeight => _itemWeight;
    }
}
