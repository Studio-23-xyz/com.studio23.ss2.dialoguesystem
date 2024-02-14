using Studio23.SS2.DialogueSystem.Utility;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.Data
{
    /// <summary>
    /// IF all normal choices are taken, shows another choice.
    /// This is useful for dialogue trees that the player needs to fully exhaust
    /// before being allowed to continue
    /// </summary>
    [CreateNodeMenu("Force Take All Choice Node"), NodeTint("#996600")]
    public class ForceTakeAllChoiceNode:DialogueChoicesNode
    {
        [Output] 
        public int FinalChoice;
        
        protected override void GetAvailableChoices()
        {
            GetAllConnectedChoiceNodes();
            Debug.Log(_availableDialogueChoices.Count);
            AddFinalChoiceIfAvailable();
            Debug.Log(_availableDialogueChoices.Count);
            RemoveUnavailableChoices();
            Debug.Log("RemoveUnavailableChoices " + _availableDialogueChoices.Count);
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