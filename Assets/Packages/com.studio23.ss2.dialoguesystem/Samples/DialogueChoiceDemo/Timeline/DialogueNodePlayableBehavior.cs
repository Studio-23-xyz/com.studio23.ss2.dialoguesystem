using System;
using Studio23.SS2.DialogueSystem.Data;
using UnityEngine;
using UnityEngine.Playables;

namespace Samples
{
    [Serializable]
    public class DialogueNodePlayableBehavior:PlayableBehaviour
    {
        public DialogueLineNodeBase Node;
        private PlayableDirector director;
        private Playable RootPlayable;
        
        public override void OnPlayableCreate(Playable playable)
        {
            RootPlayable = playable.GetGraph().GetRootPlayable(0);
            director = (playable.GetGraph().GetResolver() as PlayableDirector);
            
        }
        
        
        
        public void Pause()
        {
            // director.Pause();
            RootPlayable.SetSpeed(0);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            Pause();
            playable.GetDuration();
            
            //honestly,
            //timeline is also a dialogue view.
            var ui = playerData as DialogueBoxUI;
            if (ui != null)
            {
                ui.handleDialogueLineStarted(Node);
            }
        }
    }
}