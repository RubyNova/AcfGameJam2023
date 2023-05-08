using Environment;
using UnityEngine;

namespace SetPieceHelpers 
{
    public class EntityHider : RoomEntityBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private RoomEntityBehaviour[] _roomEntities;

        private void Update()
        {
            foreach(var entity in _roomEntities)
            {
                entity.gameObject.SetActive(false);
            }
        }

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            gameObject.SetActive(isActiveRoom);
        }

        public void EnableControlledEntities()
        {
            foreach(var entity in _roomEntities)
            {
                entity.gameObject.SetActive(true);
            }

            gameObject.SetActive(false);
        }

        public void DisableControlledEntities()
        {
            foreach(var entity in _roomEntities)
            {
                entity.gameObject.SetActive(false);
            }

            gameObject.SetActive(true);
        }
    }
}
