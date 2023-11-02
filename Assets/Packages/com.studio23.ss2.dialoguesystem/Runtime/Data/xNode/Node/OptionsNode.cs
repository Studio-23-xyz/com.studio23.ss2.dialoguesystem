using UnityEngine;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeTint("#AAAA00")]
    public class OptionsNode : Node
    {
        [Input]
        public int Entry;

        [Output(dynamicPortList = true)] public int[] Options;
    }
}
