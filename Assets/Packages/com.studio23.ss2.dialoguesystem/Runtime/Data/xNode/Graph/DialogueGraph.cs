using System;
using UnityEngine;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Dialogue System/New Dialogue Graph", fileName ="Dialogue Graph")]
    public class DialogueGraph : NodeGraph
    {
        public bool SkippableDialogue;

        [SerializeField] private DialogueNodeBase _startNode;
        public DialogueNodeBase StartNode => _startNode;
        
        private DialogueNodeBase _currentNode;

        private DialogueNodeBase _lastAddedNode;


        public void AddNewNode(DialogueNodeBase node)
        {
            nodes.Add(node);
            node.position=new Vector2(250*nodes.Count,0);

            if(_startNode==null)
            {
                _startNode = node;  
                _lastAddedNode = node;
                return;
            }

            _lastAddedNode.GetExitPort().Connect(node.GetEntryPort());
            _lastAddedNode=node;
        }

        public void HandleDialogueStarted()
        {
            //this needs to be set manually 
            //otherwise we can't have repeating dialogue
            FindStartNode();
            _currentNode = _startNode;
        }

        private void FindStartNode()
        {
            if (_startNode != null)
            {
                return;
            }
            foreach (var node in nodes)
            {
                Debug.Log("node " , node);
                if (node is DialogueStartNode startNode)
                {
                    _startNode = startNode;
                    return;
                }
            }

            Debug.LogError($"NO START NODE FOR DIALOGUE GRAPH {this}");
        }

    }
}