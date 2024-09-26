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
        }

        public void ShowInPlayMode()
        {
            PauseTimelineUntilAdvance(Node, _rootPlayable, _director, EndTime).Forget();
        }
        
        public static async UniTask PauseTimelineUntilAdvance(DialogueNodeBase node, Playable rootPlayable, PlayableDirector director, double endTime)
        {
            // Debug.Log($"pause director.time {director.time}: -> {endTime}");
            rootPlayable.SetSpeed(0);
            
            await DialogueSystem.Instance.PlayDialogue(node);
            
            //#TODO I'm unsure if this messes up sounds.
            //for now, assume that there won't be any clips parallel to dialogueUI 
            director.time = endTime;
            rootPlayable.SetSpeed(1);   
            // Debug.Log($"resume director.time {director.time}: -> {endTime}");
        }
    }
}