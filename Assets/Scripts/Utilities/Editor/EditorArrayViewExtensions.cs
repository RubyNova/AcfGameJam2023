using System;
using UnityEditor;

namespace Utilities.Editor
{
    public static class EditorArrayViewExtensions
    {
        public class EditorArrayViewSettings
        {
            public string Label { get; set; }
            public bool IsOpen { get; set; }

            public EditorArrayViewSettings(string label, bool isOpen) 
            {
                Label = label;
                IsOpen = isOpen;
            }
        }

        public static T[] UnityObjectArrayField<T>(EditorArrayViewSettings settings, T[] targetArray) where T : UnityEngine.Object
        {
            if (targetArray == null)
            {
                targetArray = new T[0];
            }

            settings.IsOpen = EditorGUILayout.Foldout(settings.IsOpen, settings.Label);
            int newSize = targetArray.Length;

            if (settings.IsOpen)
            {
                newSize = EditorGUILayout.IntField("Size", newSize);
                newSize = newSize < 0 ? 0 : newSize;

                if (newSize != targetArray.Length)
                {
                    Array.Resize(ref targetArray, newSize);
                }

                for (int i = 0; i < newSize; i++)
                {
                    targetArray[i] = (T)EditorGUILayout.ObjectField($"Value {i}", targetArray[i], typeof(T), true);
                }
            }

            return targetArray;
        }
    }
}
