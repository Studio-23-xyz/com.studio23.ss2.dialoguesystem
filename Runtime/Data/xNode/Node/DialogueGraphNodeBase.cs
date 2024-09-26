using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    public abstract class DialogueGraphNodeBase: Node
    {
        /// <summary>
        /// Called once before the graph is run the first time
        /// </summary>
        public abstract void Initialize();
    }
}