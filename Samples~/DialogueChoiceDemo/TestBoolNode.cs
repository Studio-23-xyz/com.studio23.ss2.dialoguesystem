
using XNode;

public class TestBoolNode:Node
{
    public bool IsTrue = false;

    public void DebugBool()
    {
        UnityEngine.Debug.Log($"{IsTrue}");
    }
}