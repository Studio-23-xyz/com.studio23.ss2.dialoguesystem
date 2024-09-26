using System;
using System.Collections.Generic;
using Studio23.SS2.DialogueSystem.Runtime.Data;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.Serialization;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Dialogue System/New Dial" +
                                "ogue Graph", fileName ="Dialogue Graph")]
    public class DialogueGraph : NodeGraph
    {
        public bool SkippableDialogue;

        [SerializeField] private DialogueNodeBase _startNode;
        public DialogueNodeBase StartNode => _startNode;
        
        private DialogueNodeBase _currentStartNode;
        private DialogueNodeBase _lastAddedNode;

        //This is needed if we want to make even starting the conversation conditional
        [SerializeReference, SerializeReferenceButton]
        private List<IDialogueNodeCondition> _conditions = new List<IDialogueNodeCondition>();

        //certain nodes need a callback to initialize when hitting playmode/at start in build
        //ex: condition nodes
        private bool _initialized = false;
        public event DialogueSystem.Core.DialogueSystem.DialogueGraphPlayEvent OnDialogueStarted;
        public event DialogueSystem.Core.DialogueSystem.DialogueGraphPlayEvent OnDialogueEnded;

        public override Node AddNode(Type type)
        {
            var node = base.AddNode(type);
            
            if (TryGetDefaultTable(out var defaultTable))
            {
                if (node is DialogueLineNodeBase dialogueNodeBase)
                {
                    dialogueNodeBase.SetLocalizationTable(defaultTable);
                }
            }
            
            return node;
        }

        public bool TryGetDefaultTable(out TableReference defaultTableReference)
        {
            defaultTableReference = default;
            foreach (var node in nodes)
            {
                if (node is DialogueLineNodeBase dialogueLineNodeBase)
                {
                    defaultTableReference =  dialogueLineNodeBase.GetLocalizationTable();
                    return true;
                }
            }

            return false;
        }

        public void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            FindStartNode();
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

        public void HandleDialogueStarted(DialogueNodeBase startNode)
        {
            //this needs to be set manually 
            //otherwise we can't have repeating dialogue
            FindStartNode();
            _currentStartNode = startNode;
            OnDialogueStarted?.Invoke(this, _currentStartNode);
        }

        public void HandleDialogueEnded()
        {
            OnDialogueEnded?.Invoke(this, _currentStartNode);
        }

        private DialogueStartNode FindStartNode()
        {
            if (_startNode != null)
            {
                return null;
            }
            foreach (var node in nodes)
            {
                if (node is DialogueStartNode startNode)
                {
                    _startNode = startNode;
                    return startNode;
                }
            }

            return null;
        }

        [ContextMenu("FindNodesWithoutCharacterData")]
        public void FindNodesWithoutCharacterData()
        {
            foreach (var node in nodes)
            {
                if (node is DialogueLineNodeBase lineNode &&
                    node is not DialogueChoiceNode choiceNode)
                {
                    if (lineNode.SpeakerData?.Character == null)
                    {
                        Debug.LogWarning($"{this}:{lineNode} has no characterData", lineNode);
                    }
                }
            }
        }

    }
}