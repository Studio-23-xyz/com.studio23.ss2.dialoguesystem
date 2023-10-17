using Studio23.SS2.DialogueSystem.Core;
using UnityEngine;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{

    public class DialogueGraph : NodeGraph
    {

        public bool SkippableDialogue;

        [SerializeField] private DialogueBase _startNode;
        [SerializeField] private DialogueBase _currentNode;

        private DialogueBase _lastAddedNode;

        public CharacterTable CharacterTable;

        

        public DialogueBase CurrentNode { get { return _currentNode; } }



        public void AddNewNode(DialogueBase node)
        {
            nodes.Add(node);
            node.position=new Vector2(250*nodes.Count,0);

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
            DialogueManager.Instance.OnDialogueStart?.Invoke(_currentNode);
        }

       



        public void NextNode()
        {
            if (_currentNode == null) return;
            NodePort outputPort = _currentNode.GetOutputPort("Exit");
            if (outputPort == null)
            {
                DialogueManager.Instance.OnDialogueComplete?.Invoke();
                Debug.Log("Dialogue Complete");
                _currentNode = null;
                return;
            }
            _currentNode = outputPort.Connection.node as DialogueBase; ;
            DialogueManager.Instance.OnDialogueNext?.Invoke(_currentNode);
        }

    }
}