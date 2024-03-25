using Cysharp.Threading.Tasks;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeTint("#006600"), CreateNodeMenu("DialogueStartNode")]
    public class DialogueStartNode : DialogueLineNodeBase
    {
        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "Exit")
            {
                return this;
                
            }
            return base.GetValue(port);
        }
    }
}