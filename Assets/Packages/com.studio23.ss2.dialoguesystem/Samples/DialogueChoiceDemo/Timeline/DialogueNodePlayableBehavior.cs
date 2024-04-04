using System;
using Cysharp.Threading.Tasks;
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
        private PlayableDirector _director;
        private Playable _rootPlayable;
        public double StartTime;
        public double EndTime;

        public override void OnPlayableCreate(Playable playable)
        {
            _rootPlayable = playable.GetGraph().GetRootPlayable(0);
            _director = (playable.GetGraph().GetResolver() as PlayableDirector);
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


        public void Pause()
        {
            Debug.Log($"pause director.time {_director.time}: {StartTime} -> {EndTime}");
            _rootPlayable.SetSpeed(0);
        }
        
        public void Resume()
        {
            // director.Pause();
            _rootPlayable.SetSpeed(1);
            _director.time = EndTime;
            Debug.Log($"resume director.time {_director.time}: {StartTime} -> {EndTime}");
        }

        public void ShowInPlayMode()
        {
            PauseTimelineUntilAdvance(Node, _rootPlayable, _director, EndTime).Forget();
        }
        
        public static async UniTask PauseTimelineUntilAdvance(DialogueNodeBase node, Playable rootPlayable, PlayableDirector director, double endTime)
        {
            Debug.Log($"pause director.time {director.time}: -> {endTime}");
            rootPlayable.SetSpeed(0);
            
            await DialogueSystem.Instance.PlayDialogue(node);
            
            director.time = endTime;
            rootPlayable.SetSpeed(1);   
            Debug.Log($"resume director.time {director.time}: -> {endTime}");
        }

    }
}