using System;
using UnityEngine;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Dialogue System/New Dialogue Node",fileName ="Dialogue Node")]
    [Serializable]

    [Node.NodeTint("#7A009A")]
    public class BiWayDialogueNode: DialogueLineNodeBase
    {
        [Node.Input]
        public int Entry;
        [Node.Output]
        public int Exit;
    }
}