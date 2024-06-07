using System.Collections.Generic;
using Studio23.SS2.DialogueSystem.Runtime.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateNodeMenu("Dialogue Choice")]
    public class DialogueChoiceNode:DialogueChoiceNodeBase
    {
        [SerializeReference, SerializeReferenceButton]
        private List<IDialogueNodeCondition> _conditions = new List<IDialogueNodeCondition>();

        protected override bool CheckConditionsInternal()
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
                
        public override UniTask Play()
        {
            InvokePostPlayEvents();
            return UniTask.CompletedTask;
        }
        
    }
}
