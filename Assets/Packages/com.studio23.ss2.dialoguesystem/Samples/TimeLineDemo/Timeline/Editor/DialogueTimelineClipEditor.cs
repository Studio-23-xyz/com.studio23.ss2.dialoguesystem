using Studio23.SS2.DialogueSystem.Data;
using Studio23.SS2.DialogueSystem.Utility;
using UnityEditor;
using UnityEngine;

namespace Samples.DialogueChoiceDemo.Editor
{
    [CustomEditor(typeof(DialogueTimelineClip))]
    public class DialogueTimelineClipEditor:UnityEditor.Editor
    {
        private DialogueGraph _graph;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var clip = target as DialogueTimelineClip;
            if (clip == null)
            {
                return;
            }
            var node = clip.Node;
            if (node != null)
            {
                if (node is DialogueLineNodeBase lineNodeBase)
                {
                    EditorGUILayout.HelpBox(node.DialogueLocalizedString.GetLocalizedStringInEditor(), MessageType.Info);
                }
            }

            if (_graph == null && node != null)
            {
                _graph = node.graph as DialogueGraph;
            }
            _graph = EditorGUILayout.ObjectField(_graph, typeof(DialogueGraph), false) as DialogueGraph;

            if (_graph == null)
            {
                EditorGUILayout.HelpBox("Graph or Nodes not set!", MessageType.Warning);
                return;
            }
            
            int emptyNodeNo = 0;
            var prevColor = GUI.color;

            // Loop through the nodes and create buttons for each
            foreach (var dialogueNode in _graph.nodes)
            {
                if (dialogueNode is DialogueStartNode startNode)
                {
                    //draw button
                    var dialogueLineTarget = startNode.GetNextNode() as DialogueLineNodeBase;

                    if (dialogueLineTarget == null)
                    {
                        continue;
                    }
                    
                    var buttonGroup = EditorGUILayout.BeginVertical();
                    var buttonColor =  dialogueLineTarget == clip.Node? Color.green: Color.grey;
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
                                GUILayout.Label(dialogueChoiceNodeBase.DialogueLocalizedString. GetLocalizedStringInEditor());
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
                        Undo.RecordObject(clip, "Set Dialogue Node");
                        clip.Node = dialogueLineTarget;
                        EditorUtility.SetDirty(clip);
                    }
                }
            }   
        }
    }
}