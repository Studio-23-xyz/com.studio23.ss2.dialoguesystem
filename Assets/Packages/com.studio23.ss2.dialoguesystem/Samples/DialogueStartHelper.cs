using System.Collections.Generic;
using Studio23.SS2.DialogueSystem.Core;
using Studio23.SS2.DialogueSystem.Data;
using UnityEngine;

namespace Samples
{
    public class DialogueStartHelper:MonoBehaviour
    {
        [SerializeField] private DialogueGraph _graph;
        public DialogueGraph Graph => _graph;
        public DialogueNodeBase StartNode;

        public void StartDialogueFromNode()
        {
            DialogueSystem.Instance.StartDialogue(_graph, StartNode);
        }
        
        public void StartDialogueFromDefaultStartNode()
        {
            DialogueSystem.Instance.StartDialogue(_graph);
        }
    }
    
}