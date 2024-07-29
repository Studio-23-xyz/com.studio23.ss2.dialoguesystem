using Studio23.SS2.DialogueSystem.Utility;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.Data
{
    /// <summary>
    /// IF all normal choices are taken, shows another choice.
    /// This is useful for dialogue trees that the player needs to fully exhaust
    /// before being allowed to continue
    /// </summary>
    [CreateNodeMenu("Must Take All Choices Branch Node"), NodeTint("#996600")]
    public class MustTakeAllChoicesBranch:DialogueChoicesNode
    {
        [Output(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)] 
        public DialogueChoicesNode FinalChoice;
        
        protected override void GetAvailableChoices()
        {
            FetchAllConnectedChoiceNodes();
            AddFinalChoiceIfAvailable();
            RemoveUnavailableChoices();
        }

        private void AddFinalChoiceIfAvailable()
        {
            bool allNormalChoicesTaken = true;
            foreach (var choice in AvailableDialogueChoices)
            {
                if (!choice.Taken)
                {
                    allNormalChoicesTaken = false;
                    break;
                }
            }

            if (allNormalChoicesTaken)
            {
                this.GetOutputNodesConnectedToPort("FinalChoice", _availableDialogueChoices);
            }
        }
    }
}