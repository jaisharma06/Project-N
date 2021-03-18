using UnityEditor;
using UnityEngine;

namespace Anvarat.UI
{
    [CustomEditor(typeof(Healthbar))]
    public class HealthbarEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var healthbar = target as Healthbar;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Value");
            healthbar.value = EditorGUILayout.Slider(healthbar.value, healthbar.minValue, healthbar.maxValue);
            EditorGUILayout.EndHorizontal();
        }
    }   
}
