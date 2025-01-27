using System;
using System.Threading;
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
        public DialogueNodeBase CurNode => _curNode;

        [FormerlySerializedAs("_skipDialogueLines")]
        [Header("Skip")]
        [SerializeField] private bool _skipToEndOfDialogueChain = true;
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
        public event DialogueGraphPlayEvent OnDialogueCancelledEvent;
        
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

        private CancellationTokenSource _dialogueCancelTokenSource;

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
            if(_dialogueCancelTokenSource !=null)
                _dialogueCancelTokenSource.Cancel();
            _dialogueCancelTokenSource = new CancellationTokenSource();
            graph.Initialize();
            await PlayDialogue(graph, graph.StartNode);
        }
        
        public async UniTask PlayDialogue(DialogueNodeBase startNode)
        {
            if (_dialogueCancelTokenSource != null)
                _dialogueCancelTokenSource.Cancel();
            _dialogueCancelTokenSource = new CancellationTokenSource();
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
            _skipToEndOfDialogueChain = false; 
            
            OnDialogueStarted?.Invoke(_currentGraph, startNode);
            while (CurNode != null && !_dialogueCancelTokenSource.IsCancellationRequested)
            {
                if(_skipToEndOfDialogueChain)
                {
                    if (CurNode is DialogueLineNodeBase dialogueLineNodeBase)
                    {
                        //#TODO add post play function to dialogueNodeBase
                        dialogueLineNodeBase.InvokePostPlayEvents();
                        var nextNode = CurNode.GetNextNode();
                        Debug.Log($"Skip {CurNode} -> {(nextNode == null?"null": nextNode)}");
                        _curNode = nextNode;
                        continue;
                    }
                    else
                    {
                        _skipToEndOfDialogueChain = false;
                    }      
                }
                
                Debug.Log("Play = " + CurNode, CurNode);
                await CurNode.Play();
                _curNode = CurNode.GetNextNode();
            }

            _currentGraph.HandleDialogueEnded();
            OnDialogueEnded?.Invoke(_currentGraph, startNode);
        }
        /// <summary>
        /// Traverses dialogueLineNodeBases as far as possible
        /// Warning: If you made a loop of only dialoguelinesNodes, this will hang
        /// But you shouldn't be doing that to begin with.
        /// </summary>
        [ContextMenu("Skip To End of chain")]
        public void SkipToEndOfDialogueChain()
        {
            if(CurNode != null){
                _skipToEndOfDialogueChain = true;
                CurNode.HandleDialogueAdvance();
            }
        }

        public void AdvanceDialogue()
        {
            if (CurNode != null)
            {
                CurNode.HandleDialogueAdvance();
            }
        }

        public void PickChoice(int choiceIndex)
        {
            if (CurNode != null)
            {
                if (_resetSkipAfterChoice)
                {
                    ToggleSkip(false);
                }
                CurNode.HandleChoiceSelected(choiceIndex);
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

        public void InitializeGraph(DialogueGraph graph)
        {
            var targetNode = graph.StartNode;
            if(targetNode == null)
                Debug.LogError("Can not find start node for " + graph.name);
            while (targetNode != null)
            {
                if (targetNode is DialogueLineNodeBase dialogueLineNodeBase)
                {
                    dialogueLineNodeBase.Initialize();
                    var nextNode = targetNode.GetNextNode();
                    targetNode = nextNode;
                }
            }
        }

        public void OnDialogueCancelled()
        {
            if(_dialogueCancelTokenSource != null)
                _dialogueCancelTokenSource.Cancel();
            if(CurNode != null &&  (CurNode is DialogueLineNodeBase dialogueLineNodeBase))
                dialogueLineNodeBase.CancelDialogueLine();
            OnDialogueCancelledEvent?.Invoke(_currentGraph, CurNode);
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

