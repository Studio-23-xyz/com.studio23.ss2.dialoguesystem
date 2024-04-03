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

            int emptyNodeNo = 0;
            var prevColor = GUI.color;

            // Loop through the nodes and create buttons for each
            foreach (var node in startHelper.Graph.nodes)
            {
                if (node is DialogueStartNode startNode)
                {
                    //draw button
                    var buttonGroup = EditorGUILayout.BeginVertical();
                    var buttonColor = startNode == startHelper.StartNode? Color.green: Color.grey;
                    GUI.color = buttonColor;

                    string buttonText;
                    //we need text desc of the start node
                    var nextNode = startNode.GetNextNode();
                    bool buttonPressed = false;
                    if (nextNode == null)
                    {
                        //this shouldn't happen
                        //but if it does just give a placeholder name
                        buttonText = $"<color=red>Empty StartNode{++emptyNodeNo}</color>";
                        buttonPressed = GUILayout.Button(buttonText);
                    }
                    else
                    {
                        //the method must work in editor
                        //I don't want to put unityeditor dependencies in the core
                        //so cast and handle it here.
                        if (nextNode is DialogueLineNodeBase dialogueLineNodeBase)
                        {
                            //if next node is dialogueLineNode, simply line text
                            buttonText = dialogueLineNodeBase.DialogueLocalizedString.GetLocalizedStringInEditor();
                            buttonPressed = GUILayout.Button(buttonText);

                        }else if(nextNode is DialogueChoicesNode dialogueChoicesNodeBase)
                        {
                            //if next node is a choice node
                            var choiceNodes = dialogueChoicesNodeBase.FetchAllConnectedChoiceNodes();

                            buttonText = $"Choices Branch with {choiceNodes.Count} choices)";
                            buttonPressed = GUILayout.Button(buttonText);
                            
                            GUI.color = Color.yellow;
                            foreach (var dialogueChoiceNodeBase in choiceNodes)
                            {
                                GUILayout.Label(dialogueChoiceNodeBase.DialogueLocalizedString.GetLocalizedStringInEditor());
                            }
                        }
                        else
                        {
                            //unhandled case
                            //just put child name I guess
                            buttonText = $"<color=red>Empty StartNode{++emptyNodeNo} -> {nextNode.name}</color>";
                            buttonPressed = GUILayout.Button(buttonText);
                        }
                    }
                    EditorGUILayout.EndVertical();

                    GUI.color = prevColor;
                    //handle what happens when pressed
                    if (buttonPressed)
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