using UnityEngine;
using XNode;
using static Studio23.SS2.DialogueSystem.Data.DialogueEvents;

namespace Studio23.SS2.DialogueSystem.Data
{

    public class DialogueGraph : NodeGraph
    {

        public bool SkippableDialogue;

        [SerializeField] private DialogueBase _startNode;
        [SerializeField] private DialogueBase _currentNode;

        private DialogueBase _lastAddedNode;

        public CharacterTable CharacterTable;

        public DialogueDataEvent OnDialogueStart;
        public DialogueDataEvent OnDialogueNext;
        public DialogueEvent OnDialogueComplete;

        public DialogueBase CurrentNode { get { return _currentNode; } }



        public void AddNewNode(DialogueBase node)
        {
            nodes.Add(node);
            if(_startNode==null)
            {
                _startNode = node;  
                _currentNode = node;
                _lastAddedNode = node;
                return;
            }

            _lastAddedNode.GetOutputPort("Exit").Connect(node.GetInputPort("Entry"));
            _lastAddedNode=node;
           
        }


        public void StartDialogue()
        {
            _currentNode = _startNode;
            OnDialogueStart?.Invoke(_currentNode);
        }

        public void ClearEvents()
        {
            OnDialogueStart = null;
            OnDialogueNext = null;
            OnDialogueComplete = null;
        }



        public void NextNode()
        {
            if (_currentNode == null) return;
            NodePort outputPort = _currentNode.GetOutputPort("Exit");
            if (outputPort == null)
            {
                OnDialogueComplete?.Invoke();
                Debug.Log("Dialogue Complete");
                _currentNode = null;
                return;
            }
            _currentNode = outputPort.Connection.node as DialogueBase; ;
            OnDialogueNext?.Invoke(_currentNode);
        }

    }
}