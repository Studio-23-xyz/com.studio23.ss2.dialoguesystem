using System;
using UnityEngine.Serialization;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    public abstract class DialogueConditionalNodeBase:DialogueLineNodeBase
    {
        [Node.Input]
        public DialogueConditionalBranchNode Parent;
        public bool LastConditionEvaluationStatus { get; private set; }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "Exit")
            {
                return this;
                
            }
            return base.GetValue(port);
        }
        public bool CheckConditions()
        {
            LastConditionEvaluationStatus = CheckConditionsInternal();
            return LastConditionEvaluationStatus;
        }
        protected abstract bool CheckConditionsInternal();
    }
}