
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

        [Header("Skip")]
        [SerializeField] private bool _resetSkipAfterChoice = true;
        [SerializeField] private bool _isSkipActive = false;
        [SerializeField] private bool _shouldShowLineWhenSkipped = true;
        [SerializeField] private float _showLineDurationWhenSkipping = .32f;
        public bool IsSkipActive => _isSkipActive;
        public bool ShouldShowLineWhenSkipped => _shouldShowLineWhenSkipped;
        public float ShowLineDurationWhenSkipping => _showLineDurationWhenSkipping;
        public event Action<bool> OnSkipToggled;
        public delegate void DialogueGraphPlayEvent(DialogueGraph graph, DialogueNodeBase startNode); 
        public event DialogueGraphPlayEvent OnDialogueStarted; 
        public event DialogueGraphPlayEvent OnDialogueEnded; 
        
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
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
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
        public void StartDialogue(DialogueGraph graph)
        {
            PlayDialogue(graph).Forget();
        }
        
        public void StartDialogue(DialogueNodeBase node)
        {
            PlayDialogue(node).Forget();
        }
        
        public void StartDialogue()
        {
            PlayDialogue(_currentGraph).Forget();
        }

        public async UniTask PlayDialogue(DialogueGraph graph)
        {
            graph.Initialize();
            await PlayDialogue(graph, graph.StartNode);
        }
        
        public async UniTask PlayDialogue(DialogueNodeBase startNode)
        {
            var graph = startNode.graph as DialogueGraph;
            graph.Initialize();
            await PlayDialogue(graph, startNode);
        }
        
        public async UniTask PlayDialogue(DialogueGraph graph, DialogueNodeBase startNode)
        {
            _currentGraph = graph;
            _curNode = startNode;
            _currentGraph.HandleDialogueStarted(startNode);
            _isSkipActive = false;
            
            OnDialogueStarted?.Invoke(_currentGraph, startNode);
            while (_curNode != null)
            {
                Debug.Log("Play = " + _curNode, _curNode);
                await _curNode.Play();
                
                _curNode = _curNode.GetNextNode();
            }

            _currentGraph.HandleDialogueEnded();
            OnDialogueEnded?.Invoke(_currentGraph, startNode);
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
                if (_resetSkipAfterChoice)
                {
                    ToggleSkip(false);
                }
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

        public void ToggleSkip() => ToggleSkip(!_isSkipActive);
        public void ToggleSkip(bool shouldSkipBeActive) {
            _isSkipActive = shouldSkipBeActive;
            OnSkipToggled?.Invoke(_isSkipActive);
        }

        private void OnDestroy()
        {
            if (_currentGraph != null)
            {
                _currentGraph.Cleanup();
            }
        }
    }
}

