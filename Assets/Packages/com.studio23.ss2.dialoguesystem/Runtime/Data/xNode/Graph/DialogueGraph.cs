using UnityEngine;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Dialogue System/New Dialogue Graph", fileName ="Dialogue Graph")]
    public class DialogueGraph : NodeGraph
    {

        public bool SkippableDialogue;

        [SerializeField] private DialogueNodeBase _startNode;
        public DialogueNodeBase StartNode => _startNode;
        
        private DialogueNodeBase _currentNode;

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

            var a = _lastAddedNode.GetOutputPort("Exit");
            _lastAddedNode.GetExitPort().Connect(node.GetEntryPort());
            _lastAddedNode=node;
           
        }

        public void HandleDialogueStarted()
        {
            //this needs to be set manually 
            //otherwise we can't have repeating dialogue
            _currentNode = _startNode;
        }
        
        public DialogueNodeBase GetNextNode()
        {
            if(_currentNode==null)
            {
                _currentNode = _startNode;
                return _currentNode;
            }

            NodePort outputPort = _currentNode.GetExitPort();
            if (outputPort == null || outputPort.Connection == null)
            {
                _currentNode = null;
                return _currentNode;
            }

            Debug.Log(_currentNode + " " + outputPort.Connection);
            Debug.Log(" node" + outputPort.Connection.node);
            _currentNode = outputPort.Connection.node as DialogueNodeBase;

            return _currentNode;
        }

    }
}