using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeTint(200,50,50), NodeWidth(500)]
    public class EventNode : DialogueGraphNodeBase
    {
        [Input]
        public int Entry;

        [SerializeField] UnityEvent Event;

        public void Invoke()
        {
            Event.Invoke();
        }
        public override void Initialize()
        {
            //do nothing
        }
    }
}
