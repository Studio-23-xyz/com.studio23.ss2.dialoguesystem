using System;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    public abstract class DialogueChoiceNodeBase:BiWayDialogueNode
    {
        [NonSerialized]
        public bool Taken;

        public abstract bool Evaluate();
    }
}