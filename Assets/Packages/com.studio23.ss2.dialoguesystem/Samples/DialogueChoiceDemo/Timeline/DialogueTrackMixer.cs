using Studio23.SS2.DialogueSystem.Data;
using UnityEngine;
using UnityEngine.Playables;

namespace Samples
{
    public class DialogueTrackMixer:PlayableBehaviour
    {
        private DialogueNodeBase CurNode = null;

     

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            DialogueNodePlayableBehavior targetPlayableBehavior = null;

            int numInputs = playable.GetInputCount();
            for (int i = 0; i < numInputs; i++)
            {
                var inputWeight = playable.GetInputWeight(i);
                if (inputWeight > 0)
                {
                    var inputPlayable = (ScriptPlayable<DialogueNodePlayableBehavior>)playable.GetInput(i);
                    targetPlayableBehavior = inputPlayable.GetBehaviour();
                }
            }

            if (playerData is DialogueBoxUI ui)
            {
                if (targetPlayableBehavior != null)
                {
                    //honestly,
                    //timeline is also a dialogue view.
                    if (CurNode == null)
                    {
                        Debug.Log("SHOW BECAUSE CURNODE NULL");
                        ui.ShowUI(); 
                    }

                    CurNode = targetPlayableBehavior.Node;
                    Debug.Log($"show {targetPlayableBehavior.Node}");
                    
                    targetPlayableBehavior.Show(ui);
                    // ui.handleDialogueLineStarted(node);
                }
                else
                {
                    CurNode = null;
                    ui.HideUI(); 
                }
            }
            
        }
    }
}