using Cysharp.Threading.Tasks;

namespace Studio23.SS2.DialogueSystem.Data
{
    [NodeTint("#006600"), CreateNodeMenu("DialogueStartNode")]
    public class DialogueStartNode : DialogueLineNodeBase
    {
        
        [Output] public int Exit;
        
    }
}