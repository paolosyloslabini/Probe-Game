using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ButtonMono))]
public class ButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get a reference to the target object (the script we are editing)
        ButtonMono myComponent = (ButtonMono)target;

        // Add a custom button to the inspector
        if (GUILayout.Button("Trigger Action"))
        {
            // Call the method from the target script when the button is pressed
            myComponent.TriggerAction();
        }
    }
}