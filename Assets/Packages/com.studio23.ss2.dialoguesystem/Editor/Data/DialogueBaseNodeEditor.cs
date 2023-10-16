using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CustomNodeEditor(typeof(DialogueBase))]
    public class DialogueBaseNodeEditor : NodeEditor
    {

        public Color TextColor = Color.white;
        private DialogueBase dialogueBase;
        private static GUIStyle editorLabelStyle;

        public override void OnBodyGUI()
        {
            if (dialogueBase == null) dialogueBase = target as DialogueBase;

            // Update serialized object's representation
            serializedObject.Update();

            if (editorLabelStyle == null) editorLabelStyle = new GUIStyle(EditorStyles.label);
            EditorStyles.label.normal.textColor = TextColor;
            base.OnBodyGUI();
            EditorStyles.label.normal = editorLabelStyle.normal;
        }


    }
}
