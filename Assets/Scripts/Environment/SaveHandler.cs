using UnityEngine;
using Utilities;

namespace Environment
{
    internal class SaveHandler : MonoBehaviour
    {
        [Header("Configuration"), SerializeField]
        private int _saveRoomIndex;

        [SerializeField]
        private string _playerTagName = "Player";

        public int SaveRoomIndex => _saveRoomIndex;
        public AsyncSceneSwitcher.SceneAsEnum OwningScene { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(_playerTagName))
            {
                return;
            }

            GameSaveManager.Instance.SaveGame(_saveRoomIndex + (int)OwningScene);
        }
    }
}
