using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CluesManager))]
public class CluesTriggerTEST : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CluesManager myScript = (CluesManager)target;
        if (GUILayout.Button("Refresh Display"))
        {
            myScript.RefreshDisplay();
        }
    }
}
