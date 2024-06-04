using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeTint(200,50,50), NodeWidth(500), CreateNodeMenu("Unity Event Node")]
    public class EventNode : EventNodeBase
    {
        public UnityEvent Event;

        public override void Invoke()
        {
            Event.Invoke();
        }

    }
}
