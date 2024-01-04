
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
        [Header("Execution Data")]
        [SerializeField] private DialogueNodeBase _curNode;

        public event Action<DialogueGraph> OnDialogueStarted; 
        public event Action<DialogueGraph> OnDialogueEnded; 
        
        public event Action<DialogueChoicesNode> OnDialogueChoiceStarted; 
        public event Action<DialogueChoicesNode> OnDialogueChoiceEnded; 
        public delegate void DialogueLineEvent(DialogueLineNodeBase dialogueLineNodeBase);
        
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
            _currentGraph.Initialize();
            
            _currentGraph.HandleDialogueStarted();
            OnDialogueStarted?.Invoke(_currentGraph);
            
            _curNode = _currentGraph.StartNode;
            while (_curNode != null)
            {
                Debug.Log("node = " + _curNode, _curNode);
                await _curNode.Play();
                
                _curNode = _curNode.GetNextNode();
            }

            OnDialogueEnded?.Invoke(_currentGraph);

            Debug.Log("dialogue completed");
        }

        public void AdvanceDialogue()
        {
            if (_curNode != null)
            {
                _curNode.HandleDialogueAdvance();
            }
        }

        public void PickChoice(int choiceIndex)
        {
            if (_curNode != null)
            {
                _curNode.HandleChoiceSelected(choiceIndex);
            }
        }
        
        public void HandleDialogueChoiceStarted(DialogueChoicesNode dialogueChoicesNode)
        {
            OnDialogueChoiceStarted?.Invoke(dialogueChoicesNode);
        }

        public void HandleDialogueChoiceEnded(DialogueChoicesNode dialogueChoicesNode)
        {
            OnDialogueChoiceEnded?.Invoke(dialogueChoicesNode);
        }

        private void OnDestroy()
        {
            _currentGraph.Cleanup();
        }
    }
}

