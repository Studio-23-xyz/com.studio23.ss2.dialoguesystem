using System;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Dialogue System/New Dialogue Node")]
    [Serializable]

    [NodeTint("#7A009A")]
    public class DialogueNode : DialogueBase
    {
        [Input]
        public int Entry;
        [Output]
        public int Exit;



    }

}

