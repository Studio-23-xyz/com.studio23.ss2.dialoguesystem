using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Samples
{
    public class PlayTimelineOnStart:MonoBehaviour
    {
        private void Start()
        {
            GetComponent<PlayableDirector>()
                .Play();
        }
    }
}