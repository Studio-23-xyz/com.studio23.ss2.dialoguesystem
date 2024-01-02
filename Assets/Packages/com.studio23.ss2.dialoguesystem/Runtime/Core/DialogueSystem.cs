
using System;
using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Studio23.SS2.DialogueSystem.Core
{
    public class DialogueSystem : MonoBehaviour
    {
        public static DialogueSystem Instance;

        [Header("Data")]
        [SerializeField] private DialogueGraph _currentGraph;
        [FormerlySerializedAs("_lastNode")]
        [Header("Execution Data")]
        [SerializeField] private DialogueNodeBase nextNode;
        [SerializeField] private bool _canAdvanceDialogue = false;
        [SerializeField] private int _lastChoice = -1;

        public event Action<DialogueGraph> OnDialogueStarted; 
        public event Action<DialogueGraph> OnDialogueEnded; 
        public delegate void DialogueLineEvent(DialogueLineNode dialogueLineNode);
        
        /// <summary>
        /// Subscribe to this to know when dialogue started
        /// </summary>
        public DialogueLineEvent DialogueLineStarted;

        /// <summary>
        /// Subscribe to this to know when dialogue completed
        /// </summary>
        public DialogueLineEvent DialogueLineCompleted;
        

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
        public void StartDialogue()
        {
            PlayDialogue();
        }

        public async UniTask PlayDialogue()
        {
            Debug.Log("START");
            
            _currentGraph.HandleDialogueStarted();
            OnDialogueStarted?.Invoke(_currentGraph);
            
            nextNode = _currentGraph.StartNode;
            while (nextNode != null)
            {
                var dialogueLineNode = nextNode as DialogueLineNode;
                Debug.Log("node = " + nextNode, nextNode);
                _canAdvanceDialogue = false;
                
                //#TODO perhaps dialogueUI shouldn't be responsible for advancing dialogue
                DialogueLineStarted?.Invoke(dialogueLineNode);
                while (!_canAdvanceDialogue)
                {
                    await UniTask.Yield();
                }
                DialogueLineCompleted?.Invoke(dialogueLineNode);
                
                nextNode = GetNextNode();
            }

            OnDialogueEnded?.Invoke(_currentGraph);

            Debug.Log("dialogue completed");
        }

        public void AdvanceDialogue()
        {
            _canAdvanceDialogue = true;
        }

        public void PickChoice(int choiceIndex)
        {
            
        }
         

        /// <summary>
        /// Get the next node
        /// </summary>
        /// <returns>Dialogue Base. It has all the necessary data for showing a line of dialogue</returns>
        public DialogueNodeBase GetNextNode()
        {
            nextNode= _currentGraph.GetNextNode();
            
            return nextNode;
        }

    }
}

