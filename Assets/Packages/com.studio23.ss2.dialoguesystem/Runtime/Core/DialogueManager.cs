
using Studio23.SS2.DialogueSystem.Data;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.Core
{
    public class DialogueManager : MonoBehaviour
    {

        public static DialogueManager Instance;

        [Header("Data")]
        [SerializeField] private DialogueGraph _currentGraph;

        public delegate void DialogueEvent();
        /// <summary>
        /// Subscribe to this to know when dialogue started
        /// </summary>
        public DialogueEvent OnDialogueStart;

        /// <summary>
        /// Subscribe to this to know when dialogue completed
        /// </summary>
        public DialogueEvent OnDialogueComplete;

        void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Initialize A new Dialogue Graph
        /// </summary>
        /// <param name="newGraph"></param>
        public void InitializeDialogueGraph(DialogueGraph newGraph)
        {
            _currentGraph = newGraph;
        }


        /// <summary>
        /// Start playing dialogue and it Fires the Start event
        /// </summary>
        public void PlayDialogue()
        {
            OnDialogueStart?.Invoke();
        }

        /// <summary>
        /// Get the next node
        /// </summary>
        /// <returns>Dialogue Base. It has all the necessary data for showing a line of dialogue</returns>
        public DialogueBase GetNextNode()
        {
            DialogueBase node=_currentGraph.GetNextNode();
            if(node == null)
            {
                OnDialogueComplete?.Invoke();
            }
            return node;
        }

    }
}