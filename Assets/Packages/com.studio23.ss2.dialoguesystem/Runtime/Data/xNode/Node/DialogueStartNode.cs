using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeTint("#006600"), CreateNodeMenu("DialogueStartNode")]
    public class DialogueStartNode : DialogueNodeBase
    {
        [Node.Output(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)]
        public DialogueLineNodeBase Exit;
        
        [Output(typeConstraint =  TypeConstraint.InheritedInverse)] 
        public EventNodeBase Events;

        public override DialogueNodeBase GetNextNode()
        {
            NodePort outputPort = GetExitPort();
            if (outputPort == null || outputPort.Connection == null)
            {
                return null;
            }
            return outputPort.Connection.node as DialogueNodeBase;
        }
        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "Exit")
            {
                return this;
                
            }
            return base.GetValue(port);
        }

        public override void HandleDialogueAdvance()
        {
            //do nothing
        }

        public override void HandleChoiceSelected(int choiceIndex)
        {
            //do nothing
        }

        public override  UniTask Play()
        {
            InvokePostPlayEvents();
            return UniTask.CompletedTask;
        }
        
        public override void Initialize()
        {
            //do nothing
        }
        
        public virtual void InvokePostPlayEvents()
        {
            var eventNodePort= GetOutputPort("Events");
            for (int i = 0; i < eventNodePort.ConnectionCount; i++)
            {
                var eventNodeConnection = eventNodePort.GetConnection(i);
                if (eventNodeConnection != null)
                {
                    if (eventNodeConnection.node is EventNodeBase eventNode)
                    {
                        eventNode.Invoke();
                    }
                    else
                    {
                        Debug.LogWarning($"{this} Events port no {i} is connected to {eventNodeConnection.node} but it's not an event node", this);
                    }
                }
                else
                {
                    Debug.LogWarning($"{this} Events port no {i} is connected to {eventNodeConnection} but connection null", this);
                }
            }
        }
    }
}