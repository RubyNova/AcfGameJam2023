using UnityEditor;
using UnityEngine;

namespace EngineHotfixes.Editor
{
    [CustomEditor(typeof(SpriteRestorer))]
    public class SpriteRestorerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI();
            SpriteRestorer restorer = (SpriteRestorer)target;

            if (GUILayout.Button("Restore Sprite Renderer Defaults"))
            {
                restorer.RestoreSpriteDefaults();
            }
        }
    }
}
