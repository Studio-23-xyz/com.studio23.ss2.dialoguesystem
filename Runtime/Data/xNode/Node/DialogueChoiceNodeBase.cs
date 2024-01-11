using System;
using UnityEngine.Serialization;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    public abstract class DialogueChoiceNodeBase:BiWayDialogueNode
    {
        public int DialogueChoiceIndex { get; internal set; }
        
        bool _taken;
        public bool Taken => _taken;
        public bool LastConditionEvaluationStatus { get; private set; }

        public bool CheckConditions()
        {
            LastConditionEvaluationStatus = CheckConditionsInternal();
            return LastConditionEvaluationStatus;
        }
        protected abstract bool CheckConditionsInternal();

        public void HandleChoiceTaken()
        {
            _taken = true;
        }
        public override void Initialize()
        {
            base.Initialize();
            _taken = false;
        }
    }
}