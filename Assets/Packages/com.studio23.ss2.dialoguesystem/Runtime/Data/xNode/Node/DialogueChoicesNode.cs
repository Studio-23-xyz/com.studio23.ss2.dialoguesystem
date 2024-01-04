using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeTint("#AAAA00"), CreateNodeMenu("Dialogue Multi choice")]
    public class DialogueChoicesNode : DialogueNodeBase
    {
        [Input]
        public int Entry;

        [Output] 
        public int Choices;
        
        private int _lastChoiceIndex = -1;

        private List<DialogueChoiceNode> _dialogueChoices;
        public List<DialogueChoiceNode> DialogueChoices=> _dialogueChoices;

        void GetDialogueChoices()
        {
            if (_dialogueChoices == null)
            {
                _dialogueChoices = new List<DialogueChoiceNode>();
            }
            else
            {
                _dialogueChoices.Clear();
            }

            this.GetOutputNodesConnectedToPort("Choices", _dialogueChoices);

            //trim 
            for (int i = _dialogueChoices.Count-1; i >= 0; i--)
            {
                var choice = _dialogueChoices[i];
                if (!choice.CheckConditions())
                {
                    _dialogueChoices.RemoveAt(i);
                }
            }
        }

        public override void HandleDialogueAdvance()
        {
            //do nothing
        }

        public override DialogueNodeBase GetNextNode()
        {
            if (_lastChoiceIndex < 0 || _lastChoiceIndex >= _dialogueChoices.Count)
            {
                Debug.LogError($"INDEX {_lastChoiceIndex} OUT OF RANGE for dialogue choices {this}", this);
            }
            return _dialogueChoices[_lastChoiceIndex];
        }

        public override void HandleChoiceSelected(int choiceIndex)
        {
            //no value checks. 
            //we assume that the index is valid
            _lastChoiceIndex = choiceIndex;
            if (_lastChoiceIndex >= 0 && _lastChoiceIndex < _dialogueChoices.Count)
            {
                var pickedChoice = _dialogueChoices[_lastChoiceIndex];
                pickedChoice.HandleChoiceTaken();
            }
        }

        public override async UniTask Play()
        {
            _lastChoiceIndex = -1;
            GetDialogueChoices();
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
