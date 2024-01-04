using System;
using Packages.com.studio23.ss2.dialoguesystem.Runtime.Data;

namespace Packages.com.studio23.ss2.dialoguesystem.Samples.SubtitleDemo
{
    [Serializable]
    public class TestBoolNodeInteraction:IDialogueNodeCondition
    {
        public TestBoolNode BoolNode;
        public bool Evaluate()
        {
            return BoolNode.IsTrue;
        }
    }
}