using Movement;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerContextObject
    {
        public PlayerController Controller { get; }
        public GroundMover Mover { get; }
        public InputInfo PlayerInput { get; set; }
        public bool ForceJump { get; set; }
        public Dictionary<string, Coroutine> ActiveAbilityCoroutines { get; }

        public PlayerContextObject(PlayerController controller, GroundMover mover, InputInfo playerInput, bool forceJump, Dictionary<string, Coroutine> activeAbilityCoroutines)
        {
            Controller = controller;
            Mover = mover;
            PlayerInput = playerInput;
            ForceJump = forceJump;
            ActiveAbilityCoroutines = activeAbilityCoroutines;
        }
    }
}