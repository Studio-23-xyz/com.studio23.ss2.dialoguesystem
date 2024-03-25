using Studio23.SS2.DialogueSystem.Data;
using Studio23.SS2.DialogueSystem.Utility;
using UnityEditor;
using UnityEngine;

namespace Samples.DialogueChoiceDemo.Editor
{
    [CustomEditor(typeof(DialogueStartHelper))]
    public class DialogueStartHelperEditor: UnityEditor.Editor
    {
        private DialogueStartHelper startHelper;

        private void OnEnable()
        {
            startHelper = (DialogueStartHelper)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (startHelper.Graph == null || startHelper.Graph.nodes == null)
            {
                EditorGUILayout.HelpBox("Graph or Nodes not set!", MessageType.Warning);
                return;
            }

            EditorGUILayout.Space();

            // Loop through the nodes and create buttons for each
            foreach (var node in startHelper.Graph.nodes)
            {
                if (node is DialogueStartNode startNode)
                {
                    if (GUILayout.Button(startNode.DialogueLocalizedString.GetLocalizedStringInEditor()))
                    {
                        // If button is clicked, set the node and mark as dirty
                        Undo.RecordObject(startHelper, "Set Dialogue Node");
                        startHelper.StartNode = startNode;
                        EditorUtility.SetDirty(startHelper);
                    }
                }
            }
        }
    }
}