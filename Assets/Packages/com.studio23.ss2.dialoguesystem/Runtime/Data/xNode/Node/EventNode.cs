using UnityEngine.Events;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeTint(200,50,50), NodeWidth(500)]
    public class EventNode : Node
    {
        [Input]
        public int Entry;

        public UnityEvent Event;
    }
}
