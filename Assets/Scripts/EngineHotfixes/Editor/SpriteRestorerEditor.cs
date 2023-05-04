using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace EngineHotfixes.Editor
{
    [CustomEditor(typeof(SpriteRestorer))]
    public class SpriteRestorerEditor : UnityEditor.Editor
    {
        private AnimatorController _controller;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SpriteRestorer restorer = (SpriteRestorer)target;

            GUILayout.Label("Hotfix Generation Controls");

            _controller = (AnimatorController)EditorGUILayout.ObjectField("Animator Controller Asset", _controller, typeof(AnimatorController), false);

            if (GUILayout.Button("Create New Defaults"))
            {
                var spriteRenderers = restorer.GetComponentsInChildren<SpriteRenderer>();

                SpriteRestorer.SpriteRendererDefaultConfig[] newDefaults = new SpriteRestorer.SpriteRendererDefaultConfig[spriteRenderers.Length];

                for (int i = 0; i < spriteRenderers.Length; i++)
                {
                    SpriteRenderer renderer = spriteRenderers[i];
                    Sprite sprite = renderer.sprite;

                    newDefaults[i] = new(renderer, sprite);
                }

                restorer.Defaults = newDefaults;

                GenerateTransmitters(restorer);
            }

            if (GUILayout.Button("Restore Sprite Renderer Defaults"))
            {
                restorer.RestoreSpriteDefaults();
            }
        }

        private void GenerateTransmitters(SpriteRestorer restorer)
        {
            foreach (var layer in _controller.layers)
            {
                foreach (var state in layer.stateMachine.states)
                {
                    if (state.state.behaviours.Any(x => x.GetType() == typeof(SpriteRestorerTransmitter)))
                    {
                        continue;
                    }

                    state.state.AddStateMachineBehaviour<SpriteRestorerTransmitter>();
                }
            }
        }
    }
}
