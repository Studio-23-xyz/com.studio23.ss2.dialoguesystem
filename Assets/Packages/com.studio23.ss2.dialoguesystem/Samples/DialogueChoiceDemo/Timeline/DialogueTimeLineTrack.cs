using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Samples
{
    [TrackColor(1,0,0)]
    [TrackBindingType(typeof(DialogueBoxUI))]
    [TrackClipType(typeof(DialogueTimelineClip))]
    public class DialogueTimeLineTrack:TrackAsset
    {

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {

            foreach (var clip in GetClips())
            {
                if (clip.asset is DialogueTimelineClip dialogueTimelineClip)
                {
                    dialogueTimelineClip.StartTime = clip.start;
                    dialogueTimelineClip.EndTime = clip.end;
                }
            }
            return ScriptPlayable<DialogueTrackMixer>.Create(graph, inputCount);
        }
    }
}