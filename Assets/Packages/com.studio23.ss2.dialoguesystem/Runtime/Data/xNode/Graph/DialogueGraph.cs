using System;
using System.Collections.Generic;
using Studio23.SS2.DialogueSystem.Runtime.Data;
using UnityEngine;
using UnityEngine.Localization.Tables;
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
        [SerializeField] private TableReference _defaultTable;

        public override Node AddNode(Type type)
        {
            var node = base.AddNode(type);
            
            if (node is DialogueLineNodeBase dialogueNodeBase)
            {
                dialogueNodeBase.SetLocalizationTable(GetDefaultTable());
            }
            
            return node;
        }

        public TableReference GetDefaultTable()
        {
            var firstNode = FindStartNode() as DialogueLineNodeBase;
            if (firstNode != null)
            {
                firstNode.GetLocalizationTable();
            }

            return default;
        }

        [ContextMenu("SET ALL DIALOGUE TABLES")]
        public void SetAllDialogueTables()
        {
            var table = GetDefaultTable();
            
            foreach (var node in nodes)
            {
                if (node is DialogueLineNodeBase dialogueLineNodeBase)
                {
                    if (string.IsNullOrEmpty(dialogueLineNodeBase.GetLocalizationTable().TableCollectionName))
                    {
                        Debug.LogWarning($"{dialogueLineNodeBase} replace table to {table}");
                        dialogueLineNodeBase.SetLocalizationTable(table);
                    }
                }
            }
        }

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

        public void SetAllTablesToDefault()
        {
            
        }

        private DialogueNodeBase FindStartNode()
        {
            if (_startNode != null)
            {
                return _startNode;
            }
            foreach (var node in nodes)
            {
                Debug.Log("node " , node);
                if (node is DialogueStartNode startNode)
                {
                    _startNode = startNode;
                    return _startNode;
                }
            }

            Debug.LogError($"NO START NODE FOR DIALOGUE GRAPH {this}");
            return null;
        }

    }
}