using Studio23.SS2.DialogueSystem.Data;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.View
{
    public abstract class DialogueUIBase : MonoBehaviour
    {
        public abstract void ShowUI();
        protected abstract void HideUI();
        protected abstract void HandleDialogueChoiceEnded(DialogueChoicesNode obj);
        protected abstract void HandleDialogueChoiceStarted(DialogueChoicesNode obj);
        public abstract void ShowDialogueLineImmediate(DialogueLineNodeBase node);
        protected abstract void HandleDialogueLineCompleted(DialogueLineNodeBase dialogueLineNodeBase);
        protected abstract void HandleDialogueLineStarted(DialogueLineNodeBase dialogueLineNodeBase);
    }
}