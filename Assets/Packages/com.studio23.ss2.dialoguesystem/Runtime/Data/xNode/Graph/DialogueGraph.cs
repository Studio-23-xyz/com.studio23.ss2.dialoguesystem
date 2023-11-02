using UnityEngine;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Dialogue System/New Dialogue Graph", fileName ="Dialogue Graph")]
    public class DialogueGraph : NodeGraph
    {

        public bool SkippableDialogue;

        [SerializeField] private DialogueNodeBase _startNode;
        [SerializeField] private DialogueNodeBase _currentNode;

        private DialogueNodeBase _lastAddedNode;

      

        public void AddNewNode(DialogueNodeBase node)
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


       

        public DialogueNodeBase GetNextNode()
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
            _currentNode = outputPort.Connection.node as DialogueNodeBase;

            return _currentNode;
        }

    }
}