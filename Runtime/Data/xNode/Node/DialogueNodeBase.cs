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

        public abstract void HandleDialogueCancel();

        public abstract UniTask Play();

        public abstract DialogueNodeBase GetNextNode();
    }

    public abstract class DialogueBranchingNode : DialogueNodeBase
    {
        /// <summary>
        /// Get all connected nodes.
        /// DialogueBranchingNodes can have multiple nodes connected to them
        /// This returns all of them
        /// Note: returns without checking conditions. Simply returns all connected nodes 
        /// Note: May alloc GC
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<DialogueNodeBase> GetConnectedNodes();
    }
}
