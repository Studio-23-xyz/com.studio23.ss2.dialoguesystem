using Studio23.SS2.DialogueSystem.Core;
using Studio23.SS2.DialogueSystem.Data;
using Studio23.SS2.DialogueSystem.View;

namespace Samples
{
    public class DialogueUI:DialogueUIBase
    {
        public DialogueBoxUI DialogueBox;
        public DialogueChoiceUI ChoiceUI;
        void Start()
        {
            RegisterEvents();
        }

        private void OnDestroy()
        {
            UnRegisterEvents();
        }

        private void RegisterEvents()
        {
            DialogueBox.OnDialogueAdvanced += DialogueSystem.Instance.AdvanceDialogue;
            DialogueSystem.Instance.OnDialogueStarted +=  ShowUI;
            DialogueSystem.Instance.OnDialogueEnded +=  HideUI;
            DialogueSystem.Instance.DialogueLineStarted += HandleDialogueLineStarted;
            DialogueSystem.Instance.DialogueLineCompleted += HandleDialogueLineCompleted;
            
            DialogueSystem.Instance.OnDialogueChoiceStarted += HandleDialogueChoiceStarted;
            DialogueSystem.Instance.OnDialogueChoiceEnded += HandleDialogueChoiceEnded;
        }
        
        public override void ShowDialogueLineImmediate(DialogueLineNodeBase node)
        {
            DialogueBox.ShowDialogueLineImmediate(node);
        }

        protected override void HandleDialogueChoiceEnded(DialogueChoicesNode obj)
        {
            ChoiceUI.HideUI();
        }

        protected override void HandleDialogueChoiceStarted(DialogueChoicesNode obj)
        {
            ChoiceUI.ShowChoices(obj);
        }

        private void UnRegisterEvents()
        {
            if (DialogueSystem.Instance != null)
            {
                DialogueBox.OnDialogueAdvanced -= DialogueSystem.Instance.AdvanceDialogue;
                DialogueSystem.Instance.OnDialogueStarted -=  ShowUI;
                DialogueSystem.Instance.OnDialogueEnded -=  HideUI;
                DialogueSystem.Instance.DialogueLineStarted -= HandleDialogueLineStarted;
                DialogueSystem.Instance.DialogueLineCompleted -= HandleDialogueLineCompleted;
            
                DialogueSystem.Instance.OnDialogueChoiceStarted -= HandleDialogueChoiceStarted;
                DialogueSystem.Instance.OnDialogueChoiceEnded -= HandleDialogueChoiceEnded;
            }
        }

        private void ShowUI(DialogueGraph obj) => ShowUI();
        private void ShowUI(DialogueChoicesNode obj) => ShowUI();

        public override void ShowUI()
        {
            //do nothing
        }

        protected override void HandleDialogueLineCompleted(DialogueLineNodeBase dialogueLineNodeBase)
        {
            DialogueBox.HideUI();
        }

        private void HideUI(DialogueGraph obj) => HideUI();

        protected override void HandleDialogueLineStarted(DialogueLineNodeBase dialogueLineNodeBase)
        {
            DialogueBox.ShowDialogueLine(dialogueLineNodeBase);
        }
        private void HideUI(DialogueChoicesNode obj) => HideUI();

        protected override void HideUI()
        {
            DialogueBox.HideUI();
            ChoiceUI.HideUI();
        }
    }
}