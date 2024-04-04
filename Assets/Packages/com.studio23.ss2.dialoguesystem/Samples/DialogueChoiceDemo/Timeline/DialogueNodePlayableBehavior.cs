using System;
using Studio23.SS2.DialogueSystem.Core;
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
        public double StartTime;
        public double EndTime;

        public override void OnPlayableCreate(Playable playable)
        {
            RootPlayable = playable.GetGraph().GetRootPlayable(0);
            director = (playable.GetGraph().GetResolver() as PlayableDirector);
            DialogueSystem.Instance.DialogueLineCompleted += HandleDialogueLineCompleted;
        }

        private void HandleDialogueLineCompleted(DialogueLineNodeBase dialoguelinenodebase)
        {
            //idk if subbing to events and calling resume here is a good idea
            Resume();
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);
            DialogueSystem.Instance.DialogueLineCompleted -= HandleDialogueLineCompleted;
        }


        public void Pause()
        {
            Debug.Log("PAUSE ");
            RootPlayable.SetSpeed(0);
        }
        
        public void Resume()
        {
            // director.Pause();
            Debug.Log("RESUME ");
            RootPlayable.SetSpeed(1);
        }

        public void Show(DialogueBoxUI defaultUI)
        {
            if (Application.isPlaying)
            {
                Pause();
                DialogueSystem.Instance.StartDialogue(Node);
            }
            else
            {
                //editor preview of first line
                defaultUI.handleDialogueLineStarted(Node);
            }
        }
        

        // public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        // {
        //     base.ProcessFrame(playable, info, playerData);
        //     Pause();
        //     playable.GetDuration();
        //     
        //     //honestly,
        //     //timeline is also a dialogue view.
        //     var ui = playerData as DialogueBoxUI;
        //     if (ui != null)
        //     {
        //         ui.handleDialogueLineStarted(Node);
        //     }
        // }
    }
}