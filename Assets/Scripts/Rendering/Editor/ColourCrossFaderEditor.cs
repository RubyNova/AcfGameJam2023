using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Editor;

namespace Rendering.Editor
{
    [CustomEditor(typeof(ColourCrossFader))]
    public class ColourCrossFaderEditor : UnityEditor.Editor
    {
        private EditorArrayViewExtensions.EditorArrayViewSettings _materialSettings;
        private EditorArrayViewExtensions.EditorArrayViewSettings _spriteRendererSettings;
        private EditorArrayViewExtensions.EditorArrayViewSettings _uiImageSettings;

        public ColourCrossFaderEditor()
        {
            _materialSettings = new("Materials", false);
            _spriteRendererSettings = new("Sprite Renderers", false);
            _uiImageSettings = new("UI Images", false);
        }

        public override void OnInspectorGUI()
        { 
            var crossFader = (ColourCrossFader)target;
            GUILayout.Label("Dependencies & Configuration");
            
            base.OnInspectorGUI();

            crossFader.ComponentType = (ColourCrossFader.TargetType)EditorGUILayout.EnumPopup("Component Type", crossFader.ComponentType);

            switch (crossFader.ComponentType)
            {
                case ColourCrossFader.TargetType.Material:
                crossFader.Materials = EditorArrayViewExtensions.UnityObjectArrayField(_materialSettings, crossFader.Materials);
                    break;
                case ColourCrossFader.TargetType.SpriteRenderer:
                crossFader.SpriteRenderers = EditorArrayViewExtensions.UnityObjectArrayField(_spriteRendererSettings, crossFader.SpriteRenderers);
                    break;
                case ColourCrossFader.TargetType.UIImage:
                crossFader.UIImages =  EditorArrayViewExtensions.UnityObjectArrayField(_uiImageSettings, crossFader.UIImages);
                    break;
            }
        }
    }
}
