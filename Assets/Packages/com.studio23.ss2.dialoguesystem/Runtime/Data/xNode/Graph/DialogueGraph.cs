using System;
using System.Collections.Generic;
using Studio23.SS2.DialogueSystem.Runtime.Data;
using UnityEngine;
using UnityEngine.Serialization;
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

        //This is needed if we want to make even starting the conversation conditional
        [SerializeReference, SerializeReferenceButton]
        private List<IDialogueNodeCondition> _conditions = new List<IDialogueNodeCondition>();

        //certain nodes need a callback to initialize when hitting playmode/at start in build
        //ex: condition nodes
        private bool _initialized = false;
        public event Action<DialogueGraph> OnDialogueStarted;
        public event Action<DialogueGraph> OnDialogueEnded;

        public void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            foreach (var node in nodes)
            {
                if (node is DialogueGraphNodeBase dialogueGraphNode)
                {
                    dialogueGraphNode.Initialize();
                } 
            }
        }
        public void Cleanup()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }
        
        public bool ConditionsValid()
        {
            foreach (var condition in _conditions)
            {
                if (!condition.Evaluate())
                {
                    return false;
                }
            }

            return true;
        }

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
            OnDialogueStarted?.Invoke(this);
        }

        public void HandleDialogueEnded()
        {
            OnDialogueEnded?.Invoke(this);
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