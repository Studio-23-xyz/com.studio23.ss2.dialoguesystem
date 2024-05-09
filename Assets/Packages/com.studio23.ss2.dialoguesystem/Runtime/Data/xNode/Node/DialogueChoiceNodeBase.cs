using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Serialization;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    public abstract class DialogueChoiceNodeBase:DialogueLineNodeBase
    {
        [Node.Input]
        public DialogueChoicesNode ParentChoice;

        public int DialogueChoiceIndex { get; internal set; }
        
        bool _taken;
        public bool Taken => _taken;
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

        public override UniTask Play()
        {
            InvokePostPlayEvents();
            return UniTask.CompletedTask;
        }

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