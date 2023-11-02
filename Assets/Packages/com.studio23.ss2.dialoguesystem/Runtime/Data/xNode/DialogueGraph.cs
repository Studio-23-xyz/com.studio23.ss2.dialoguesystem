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

      

        public void AddNewNode(DialogueBase node)
        {
            nodes.Add(node);
            node.position=new Vector2(250*nodes.Count,0);

            if(_startNode==null)
            {
                _startNode = node;  
                _lastAddedNode = node;
                return;
            }

            _lastAddedNode.GetOutputPort("Exit").Connect(node.GetInputPort("Entry"));
            _lastAddedNode=node;
           
        }


       

        public DialogueBase GetNextNode()
        {
            
            if(_currentNode==null)
            {
                _currentNode = _startNode;
                return _currentNode;
            }

            NodePort outputPort = _currentNode.GetOutputPort("Exit");
            if (outputPort == null)
            {
                _currentNode = null;
                return _currentNode;
            }
            _currentNode = outputPort.Connection.node as DialogueBase;

            return _currentNode;
        }

    }
}