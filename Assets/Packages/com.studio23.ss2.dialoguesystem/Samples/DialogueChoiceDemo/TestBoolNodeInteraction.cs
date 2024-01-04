using System;
using Packages.com.studio23.ss2.dialoguesystem.Runtime.Data;

[Serializable]
public class TestBoolNodeInteraction:IDialogueNodeCondition
{
    public TestBoolNode BoolNode;
    public bool Evaluate()
    {
        return BoolNode.IsTrue;
    }
}