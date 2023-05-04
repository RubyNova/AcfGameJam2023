using System;
using UnityEngine;

namespace EngineHotfixes
{
    public class SpriteRestorerTransmitter : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<SpriteRestorer>().RestoreSpriteDefaults();
        }
    }
}
