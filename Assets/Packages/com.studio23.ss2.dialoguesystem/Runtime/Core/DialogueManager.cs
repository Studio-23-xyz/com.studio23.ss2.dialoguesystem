
using Studio23.SS2.DialogueSystem.Data;
using UnityEngine;
using static Studio23.SS2.DialogueSystem.Data.DialogueEvents;

namespace Studio23.SS2.DialogueSystem.Core
{
    public class DialogueManager : MonoBehaviour
    {

        public static DialogueManager Instance;

        [Header("Data")]
        [SerializeField] private DialogueGraph _currentGraph;

        public DialogueDataEvent OnDialogueStart;
        public DialogueDataEvent OnDialogueNext;
        public DialogueEvent OnDialogueComplete;

        void Awake()
        {
            Instance = this;
        }

        public void ClearEvents()
        {
            OnDialogueStart = null;
            OnDialogueNext = null;
            OnDialogueComplete = null;
        }

        public void ChangeDialogueGraph(DialogueGraph newGraph)
        {
            _currentGraph = newGraph;
        }

        public void PlayDialogue()
        {
            _currentGraph.StartDialogue();
        }

        public void GetNextNode()
        {
            _currentGraph.NextNode();
        }
    }
}