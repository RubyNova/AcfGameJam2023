using Environment;
using System;
using UnityEngine;

namespace SetPieceHelpers
{
    public class AnimatorFlagSetter : RoomEntityBehaviour
    {
        [Serializable]
        public class NameBoolFlagPair
        {
            [SerializeField]
            private string _flagName;

            [SerializeField]
            private bool _flagValue;

            public string FlagName => _flagName;
            public bool FlagValue => _flagValue;

            public void Deconstruct(out string name, out bool value)
            {
                name = FlagName;
                value = FlagValue;
            }
        }

        [Serializable]
        public class NameFloatFlagPair
        {
            [SerializeField]
            private string _flagName;

            [SerializeField]
            private float _flagValue;

            public string FlagName => _flagName;
            public float FlagValue => _flagValue;

            public void Deconstruct(out string name, out float value)
            {
                name = FlagName;
                value = FlagValue;
            }
        }

        [Header("Dependencies"), SerializeField]
        private Animator _target;

        [Header("Configuration"), SerializeField]
        private string[] _triggerFlagNames;

        [SerializeField]
        private NameBoolFlagPair[] _boolFlagData;

        [SerializeField]
        private NameFloatFlagPair[] _floatFlagData;

        private bool _isActive = false;

        public override void NotifyActiveStatus(bool isActiveRoom, Room roomContext, Vector2 playerEntryPosition = default)
        {
            _isActive = isActiveRoom;
        }

        public void ApplyFlags()
        {
            if (!_isActive)
            {
                return;
            }

            foreach (var flag in _triggerFlagNames)
            {
                _target.SetTrigger(flag);
            }

            foreach (var (name, value) in _boolFlagData)
            {
                _target.SetBool(name, value);
            }

            foreach (var (name, value) in _floatFlagData)
            {
                _target.SetFloat(name, value);
            }
        }
    }
}
