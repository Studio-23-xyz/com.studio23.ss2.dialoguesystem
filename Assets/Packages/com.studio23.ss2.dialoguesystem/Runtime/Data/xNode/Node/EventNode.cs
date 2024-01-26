using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeTint(200,50,50), NodeWidth(500), CreateNodeMenu("Unity Event Node")]
    public class EventNode : EventNodeBase
    {
        [SerializeField] UnityEvent _event;

        public override void Invoke()
        {
            _event.Invoke();
        }

    }
}
