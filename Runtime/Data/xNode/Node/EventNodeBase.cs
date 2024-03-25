namespace Studio23.SS2.DialogueSystem.Data
{
    public abstract class EventNodeBase : DialogueGraphNodeBase
    {
        [Input(typeConstraint = TypeConstraint.Strict)]
        public EventNodeBase TriggeringNode;

        public abstract void Invoke();

        public override void Initialize()
        {
            //do nothing
        }
    }
}