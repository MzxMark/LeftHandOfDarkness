using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueManager))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueManager myScript = (DialogueManager)target;
        if (GUILayout.Button("Display UI"))
        {
            myScript.DisplayUIElements();
        }
        if (GUILayout.Button("Print player text"))
        {
            myScript.PrintPlayerText("I am the player character.");
        }
        if (GUILayout.Button("Print npc text"))
        {
            myScript.PrintNPCText("I am an NPC.", "Test NPC");
        }
        if (GUILayout.Button("Prompt 2 choices"))
        {
            myScript.PromptChoice("Choice 1", "Choice 2");
        }
        if (GUILayout.Button("Prompt 3 choices"))
        {
            myScript.PromptChoice("Choice 1", "Choice 2", "Choice 3");
        }
        if (GUILayout.Button("End dialogue"))
        {
            myScript.EndDialogue();
        }
    }
}
