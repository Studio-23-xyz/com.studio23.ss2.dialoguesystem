using System;
using Studio23.SS2.DialogueSystem.Runtime.Data;

[Serializable]
public class TestBoolNodeInteraction:IDialogueNodeCondition
{
    public TestBoolNode BoolNode;
    public bool Evaluate()
    {
        return BoolNode.IsTrue;
    }
}