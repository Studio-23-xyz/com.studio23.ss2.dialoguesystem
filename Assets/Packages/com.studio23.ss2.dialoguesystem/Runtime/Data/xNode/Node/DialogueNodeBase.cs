using UnityEngine;
using XNode;


namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeWidth(333)]
    public class DialogueNodeBase : Node
    {

        [Output(dynamicPortList =true)] public int[] Events;


        [Header("Character Data")]
        public string ID;
        public string Reaction;


        public string FMODEvent;


        public NodePort GetExitPort()
        {
            return GetOutputPort("Exit");
        }

        public NodePort GetEntryPort()
        {
            return GetInputPort("Entry") ;
        }
    }
}
