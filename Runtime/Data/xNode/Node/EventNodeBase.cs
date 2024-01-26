namespace Studio23.SS2.DialogueSystem.Data
{
    public abstract class EventNodeBase : DialogueGraphNodeBase
    {
        [Input]
        public int Entry;

        public abstract void Invoke();

        public override void Initialize()
        {
            //do nothing
        }
    }
}