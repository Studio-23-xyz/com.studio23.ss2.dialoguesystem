using UnityEngine.Events;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    public class EventNode : Node
    {
        [Input]
        public int Entry;

        public UnityEvent Event;


    }
}
