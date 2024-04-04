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

            var director = playable.GetGraph().GetResolver() as PlayableDirector;
           
            if (playerData is DialogueBoxUI ui)
            {
                if (targetPlayableBehavior != null && 
                    director.time < director.duration)//this can retrigger if dialogue clip is the last clip. Hence check
                {
                    if (CurNode != targetPlayableBehavior.Node)
                    {
                        //honestly,
                        //timeline is also a dialogue view.
                        if (CurNode == null)
                        {
                            ui.ShowUI(); 
                        }
                        
                        Debug.Log($"show {CurNode} -> {targetPlayableBehavior.Node}");
                        CurNode = targetPlayableBehavior.Node;
                    
                        if (Application.isPlaying)
                        {
                            targetPlayableBehavior.Show(ui);
                        }
                        else
                        {
                            if (CurNode is DialogueLineNodeBase dialogueLineNodeBase)
                            {
                                ui.ShowDialogueLineImmediate(dialogueLineNodeBase);
                            }
                        }
                    }
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