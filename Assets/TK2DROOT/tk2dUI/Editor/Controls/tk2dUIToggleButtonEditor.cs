using UnityEngine;
using UnityEditor;
using System.Collections;

[CanEditMultipleObjects]
[CustomEditor(typeof(tk2dUIToggleButton))]
public class tk2dUIToggleButtonEditor : tk2dUIBaseItemControlEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        tk2dUIToggleButton toggleBtn = (tk2dUIToggleButton)target;

        toggleBtn.onStateGO = tk2dUICustomEditorGUILayout.SceneObjectField("On State GameObject", toggleBtn.onStateGO,target);
        toggleBtn.offStateGO = tk2dUICustomEditorGUILayout.SceneObjectField("Off State GameObject", toggleBtn.offStateGO,target);

        toggleBtn.activateOnPress = EditorGUILayout.Toggle("Activate On Press", toggleBtn.activateOnPress);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(toggleBtn);
        }
    }

}