using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Rendering.Editor
{
    [CustomEditor(typeof(ColourCrossFader))]
    public class COlourCrossFaderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var crossFader = (ColourCrossFader)target;
            GUILayout.Label("Dependencies & Configuration");

            crossFader.ComponentType = (ColourCrossFader.TargetType)EditorGUILayout.EnumPopup("Component Type", crossFader.ComponentType);

            switch (crossFader.ComponentType)
            {
                case ColourCrossFader.TargetType.Material:
                crossFader.Material = (Material)EditorGUILayout.ObjectField("Material", crossFader.Material, typeof(Material), false);
                    break;
                case ColourCrossFader.TargetType.SpriteRenderer:
                crossFader.SpriteRenderer = (SpriteRenderer)EditorGUILayout.ObjectField("Sprite Renderer", crossFader.SpriteRenderer, typeof(SpriteRenderer), false);
                    break;
                case ColourCrossFader.TargetType.UIImage:
                crossFader.UIImage = (Image)EditorGUILayout.ObjectField("UI Image", crossFader.UIImage, typeof(Image), false);
                    break;
            }

            base.OnInspectorGUI();
        }
    }
}
