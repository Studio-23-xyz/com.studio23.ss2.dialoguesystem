using System.Collections.Generic;
using Packages.com.studio23.ss2.dialoguesystem.Runtime.Data;
using UnityEngine;
namespace Studio23.SS2.DialogueSystem.Data
{
    public class DialogueChoiceNode:DialogueChoiceNodeBase
    {
        [SerializeReference, SerializeReferenceButton]private List<IDialogueNodeCondition> _conditions = new List<IDialogueNodeCondition>();
        
        public override bool Evaluate()
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