using System.Collections.Generic;
using Studio23.SS2.DialogueSystem.Runtime.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateNodeMenu("Dialogue Conditional node"), NodeTint("#0055AA")]
    public class DialogueConditionalNode:DialogueConditionalNodeBase
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
        
        
    }
}
