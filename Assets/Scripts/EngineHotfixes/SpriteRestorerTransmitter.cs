using UnityEngine;

namespace EngineHotfixes
{
    public class SpriteRestorerTransmitter : StateMachineBehaviour
    {
        [Header("Dependencies"), SerializeField]
        private SpriteRestorer _restorer;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _restorer.RestoreSpriteDefaults();
        }
    }
}
