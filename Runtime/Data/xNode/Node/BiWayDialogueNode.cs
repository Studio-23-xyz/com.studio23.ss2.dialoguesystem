using System;
using UnityEngine;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Dialogue System/New Dialogue Node",fileName ="Dialogue Node")]
    [Serializable]
    [Node.NodeTint("#7A009A"), CreateNodeMenu("Dialogue Line")]
    public class BiWayDialogueNode: DialogueLineNodeBase
    {
        [Node.Input]
        public DialogueLineNodeBase Entry;



    }
}