using System;
using Studio23.SS2.DialogueSystem.Data;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Samples
{
    [Serializable]
    [TrackBindingType(typeof(DialogueBoxUI))]
    public class DialogueTimelineClip: PlayableAsset, ITimelineClipAsset
    {
        public DialogueLineNodeBase Node;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<DialogueNodePlayableBehavior>.Create(graph);
            var dialogueBehavior = playable.GetBehaviour();
            dialogueBehavior.Node = Node;
            return playable;
        }

        public override double duration => 1;
        public ClipCaps clipCaps => ClipCaps.None;
    }
}