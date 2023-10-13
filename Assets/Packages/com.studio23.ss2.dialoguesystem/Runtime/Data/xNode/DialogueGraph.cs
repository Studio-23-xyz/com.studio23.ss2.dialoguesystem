using UnityEngine;
using XNode;
using static Studio23.SS2.DialogueSystem.Data.DialogueEvents;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Dialogue System/New Dialogue Graph")]
    public class DialogueGraph : NodeGraph
    {
        [SerializeField]private DialogueBase _startNode;
        [SerializeField]private DialogueBase _currentNode;

        public CharacterTable CharacterTable {  get; private set; }

        public DialogueEvent OnDialogueStart;
        public DialogueEvent OnDialogueNext;
        public DialogueEvent OnDialogueComplete;

        public DialogueBase CurrentNode { get { return _currentNode; } }

        private void OnEnable()
        {
            Initialize();
        }

        
        public void Initialize()
        {
            CharacterTable = Resources.Load<CharacterTable>("DialogueSystem/CharacterTable");
        }

        public void StartDialogue()
        {
            _currentNode = _startNode;
            OnDialogueStart?.Invoke();
        }

        public void ClearEvents()
        {
            OnDialogueStart = null; 
            OnDialogueNext=null;
            OnDialogueComplete = null;
        }



        public DialogueBase AddDialogeNode()
        {
            return _startNode == null ? AddStartNode() : AddNode<DialogueBase>();
        }

        public DialogueBase AddStartNode()
        {
            DialogueBase node = AddNode<StartNode>();
            _startNode=node;
            return node;
        }


        public DialogueBase AddEndNode()
        {
            return AddNode<EndNode>();
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
            OnDialogueNext?.Invoke();
        }




    }
}