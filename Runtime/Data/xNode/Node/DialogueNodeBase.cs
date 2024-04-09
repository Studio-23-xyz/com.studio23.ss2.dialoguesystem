using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Utility;
using UnityEngine;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeWidth(333)]
    public abstract class DialogueNodeBase : DialogueGraphNodeBase
    {
        public NodePort GetExitPort()
        {
            return GetOutputPort("Exit");
        }

        public NodePort GetEntryPort()
        {
            return GetInputPort("Entry");
        }

        public abstract void HandleDialogueAdvance();
        public abstract void HandleChoiceSelected(int choiceIndex);

        public abstract UniTask Play();

        public abstract DialogueNodeBase GetNextNode();
    }
}
