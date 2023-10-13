
using Studio23.SS2.DialogueSystem.Data;
using Studio23.SS2.DialogueSystem.UI;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.Core
{
    public class DialogueManager : MonoBehaviour
    {

        public static DialogueManager Instance;

        [Header("UI")]
        public DialogueUI DialogueUI;

        [Header("Data")]
        [SerializeField] private DialogueGraph _currentGraph;


        void Awake()
        {
            Instance = this;
        }


        public void ChangeDialogueGraph(DialogueGraph newGraph)
        {
            _currentGraph = newGraph;
        }

        public void PlayDialogue()
        {
            DialogueUI.RegisterGraph(_currentGraph);
            _currentGraph.StartDialogue();
        }

        

















    }
}