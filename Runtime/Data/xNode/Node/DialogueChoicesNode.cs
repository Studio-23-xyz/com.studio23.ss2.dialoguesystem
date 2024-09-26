using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Studio23.SS2.DialogueSystem.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeTint("#AAAA00"), CreateNodeMenu("Dialogue Choice Branch")]
    public class DialogueChoicesNode : DialogueBranchingNode
    {
        [Input]
        public DialogueLineNodeBase Entry;
        [Output]
        public DialogueChoicesNode Choices;
        
        private int _lastChoiceIndex = -1;

        protected List<DialogueChoiceNodeBase> _availableDialogueChoices;
        public List<DialogueChoiceNodeBase> AvailableDialogueChoices => _availableDialogueChoices;
        [Output] public DialogueChoicesNode ForceExitChoice;

        [CanBeNull] public DialogueChoiceNode GetForceExitNode()
        {
            NodePort outputPort = GetOutputPort("ForceExitChoice");
            if (outputPort == null || outputPort.Connection == null)
            {
                return null;
            }
            return outputPort.Connection.node as DialogueChoiceNode;
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "Choices")
            {
                return this;
            }
            if (port.fieldName == "ForceExitPort")
            {
                return this;
            }
            return base.GetValue(port);
        }

        protected virtual void PrepareDialogueChoices()
        {
            GetAvailableChoices();
            SetChoiceIndices();
        }

        protected virtual void GetAvailableChoices()
        {
            FetchAllConnectedChoiceNodes();
            RemoveUnavailableChoices();
        }


        public List<DialogueChoiceNodeBase> FetchAllConnectedChoiceNodes()
        {
            if (_availableDialogueChoices == null)
            {
                _availableDialogueChoices = new List<DialogueChoiceNodeBase>();
            }
            else
            {
                _availableDialogueChoices.Clear();
            }

            this.GetOutputNodesConnectedToPort("Choices", _availableDialogueChoices);
            return _availableDialogueChoices;
        }

        private void SetChoiceIndices()
        {
            for (int i = 0; i < _availableDialogueChoices.Count; i++)
            {
                _availableDialogueChoices[i].DialogueChoiceIndex = i;
            }

            var forceExitNode = GetForceExitNode();
            if (forceExitNode != null)
            {
                forceExitNode.DialogueChoiceIndex = -1;
            }
        }

        protected void RemoveUnavailableChoices()
        {
            for (int i = _availableDialogueChoices.Count-1; i >= 0; i--)
            {
                var choice = _availableDialogueChoices[i];
                if (!choice.CheckConditions())
                {
                    _availableDialogueChoices.RemoveAt(i);
                }
            }
        }

        public override void HandleDialogueAdvance()
        {
            //do nothing
        }

        public override DialogueNodeBase GetNextNode()
        {
            if (_lastChoiceIndex == -1)
            {
                var forceExitNode = GetForceExitNode();
                if (forceExitNode != null)
                {
                    return forceExitNode;
                }
            }
            if (_lastChoiceIndex < 0 || _lastChoiceIndex >= _availableDialogueChoices.Count)
            {
                Debug.LogError($"INDEX {_lastChoiceIndex} OUT OF RANGE for dialogue choices {this}", this);
            }
            return _availableDialogueChoices[_lastChoiceIndex];
        }

        /// <inheritdoc />
        public override IEnumerable<DialogueNodeBase> GetConnectedNodes()
        {
            //#TODO can be optimized
            //not priority since only exporter uses this func
            foreach (var choiceNode in FetchAllConnectedChoiceNodes())
            {
                yield return choiceNode;
            }
            var forceExitNode = GetForceExitNode();
            if (forceExitNode != null)
            {
                yield return forceExitNode;
            }
        }

        public override void HandleChoiceSelected(int choiceIndex)
        {
            //no value checks. 
            //we assume that the index is valid
            _lastChoiceIndex = choiceIndex;
            if (_lastChoiceIndex >= 0 && _lastChoiceIndex < _availableDialogueChoices.Count)
            {
                var pickedChoice = _availableDialogueChoices[_lastChoiceIndex];
                pickedChoice.HandleChoiceTaken();
            }else if (_lastChoiceIndex == -1)
            {
                var node = GetForceExitNode();
                if (node != null)
                {
                    node.HandleChoiceTaken();
                }
            }
        }

        public override async UniTask Play()
        {
            _lastChoiceIndex = -69;
            PrepareDialogueChoices();
            Core.DialogueSystem.Instance.HandleDialogueChoiceStarted(this);
            
            while (_lastChoiceIndex < 0 && _lastChoiceIndex != -1)
            {
                await UniTask.Yield();
            }
            
            Core.DialogueSystem.Instance.HandleDialogueChoiceEnded(this);
        }

        public override void Initialize()
        {
            //do nothing
        }
    }
}
