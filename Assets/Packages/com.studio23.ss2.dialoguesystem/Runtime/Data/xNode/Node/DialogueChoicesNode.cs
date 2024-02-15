using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeTint("#AAAA00"), CreateNodeMenu("Dialogue Choice Branch")]
    public class DialogueChoicesNode : DialogueNodeBase
    {
        [Input]
        public DialogueLineNodeBase Entry;
        [Output]
        public DialogueChoicesNode Choices;
        
        private int _lastChoiceIndex = -1;

        protected List<DialogueChoiceNodeBase> _availableDialogueChoices;
        public List<DialogueChoiceNodeBase> AvailableDialogueChoices => _availableDialogueChoices;

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "Choices")
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
            GetAllConnectedChoiceNodes();
            RemoveUnavailableChoices();
        }


        protected virtual void GetAllConnectedChoiceNodes()
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
        }

        private void SetChoiceIndices()
        {
            for (int i = 0; i < _availableDialogueChoices.Count; i++)
            {
                _availableDialogueChoices[i].DialogueChoiceIndex = i;
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
            if (_lastChoiceIndex < 0 || _lastChoiceIndex >= _availableDialogueChoices.Count)
            {
                Debug.LogError($"INDEX {_lastChoiceIndex} OUT OF RANGE for dialogue choices {this}", this);
            }
            return _availableDialogueChoices[_lastChoiceIndex];
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
            }
        }

        public override async UniTask Play()
        {
            _lastChoiceIndex = -1;
            PrepareDialogueChoices();
            Core.DialogueSystem.Instance.HandleDialogueChoiceStarted(this);
            
            while (_lastChoiceIndex < 0)
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
