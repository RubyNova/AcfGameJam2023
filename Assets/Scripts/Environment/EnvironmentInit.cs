using Environment;
using Player;
using UnityEngine;

namespace Environment
{
    public class EnvironmentInit : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private Room _startingRoom;

        [SerializeField]
        private PlayerController _playerController;

        // Start is called before the first frame update
        void Start()
        {
            _startingRoom.BecomeActiveRoom(_playerController.transform.position);
        }
    }
}
