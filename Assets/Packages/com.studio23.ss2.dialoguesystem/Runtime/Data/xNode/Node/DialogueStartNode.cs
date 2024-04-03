using Cysharp.Threading.Tasks;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeTint("#006600"), CreateNodeMenu("DialogueStartNode")]
    public class DialogueStartNode : DialogueNodeBase
    {
        [Node.Output(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)]
        public DialogueLineNodeBase Exit;
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
            return UniTask.CompletedTask;
        }
        
        public override void Initialize()
        {
            //do nothing
        }
    }
}