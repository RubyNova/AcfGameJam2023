using Environment;
using UnityEngine;

namespace Environment
{
    public class EnvironmentInit : MonoBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private Room _startingRoom;

        // Start is called before the first frame update
        void Start()
        {
            _startingRoom.BecomeActiveRoom();
        }
    }
}
