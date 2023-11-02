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

        [TextArea(4, 10)]
        public string DialogueText;
        public string FMODEvent;



    }
}
