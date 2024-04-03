using Studio23.SS2.DialogueSystem.Data;
using UnityEngine.Playables;

namespace Samples
{
    public class DialogueTrackMixer:PlayableBehaviour
    {
        private DialogueNodeBase CurNode;
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            DialogueLineNodeBase node = null;

            int numInputs = playable.GetInputCount();
            for (int i = 0; i < numInputs; i++)
            {
                var inputWeight = playable.GetInputWeight(i);
                if (inputWeight > 0)
                {
                    var inputPlayable = (ScriptPlayable<DialogueNodePlayableBehavior>)playable.GetInput(i);

                    var input = inputPlayable.GetBehaviour();
                    node = input.Node;
                }
            }

            if (playerData is DialogueBoxUI ui)
            {
                if (node != null)
                {
                    //honestly,
                    //timeline is also a dialogue view.
                    if (CurNode == null)
                    {
                        ui.ShowUI(); 
                    }

                    CurNode = node;
                    ui.handleDialogueLineStarted(node);
                }
                else
                {
                    CurNode = node;
                    if (CurNode == null)
                    {
                        ui.HideUI(); 
                    }
                }
            }
            
        }
    }
}