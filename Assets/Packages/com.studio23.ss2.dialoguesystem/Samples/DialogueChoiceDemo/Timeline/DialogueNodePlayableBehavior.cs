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
            // DialogueSystem.Instance.DialogueLineCompleted += HandleDialogueLineCompleted;
        }

        // private void HandleDialogueLineCompleted(DialogueLineNodeBase dialoguelinenodebase)
        // {
        //     //idk if subbing to events and calling resume here is a good idea
        //     Resume();
        // }

        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);
            // DialogueSystem.Instance.DialogueLineCompleted -= HandleDialogueLineCompleted;
        }


        // public void Pause()
        // {
        //     Debug.Log($"pause director.time {director.time}: {StartTime} -> {EndTime}");
        //     RootPlayable.SetSpeed(0);
        // }
        
        // public void Resume()
        // {
        //     // director.Pause();
        //     RootPlayable.SetSpeed(1);
        //     director.time = EndTime;
        //     Debug.Log($"resume director.time {director.time}: {StartTime} -> {EndTime}");
        // }

        public void Show(DialogueBoxUI defaultUI)
        {
            if (Application.isPlaying)
            {
                DialogueBoxUI.Instance.Pause(RootPlayable, EndTime);
                DialogueSystem.Instance.StartDialogue(Node);
            }
            else
            {
                //editor preview of first line
                defaultUI.handleDialogueLineStarted(Node);
            }
        }
    }
}