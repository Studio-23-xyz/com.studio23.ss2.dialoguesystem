


using UnityEngine;
using XNode;


namespace Studio23.SS2.DialogueSystem.Data
{
    public class DialogueBase : Node
    {

        [HideInInspector]
        public string ID;

        [Header("Public Properties")]
        public string CharacterID;
        public string CharacterReaction;
        [SerializeField]private CharacterTable.CharacterInfo _characterInfo;


        [TextArea(3, 10)]
        public string DialogueText;
        public string FMODEventPath;

        private void OnEnable()
        {
            base.OnEnable();
            DialogueGraph dialogueGraph = graph as DialogueGraph;
            _characterInfo = dialogueGraph.CharacterTable.GetCharacterInfo(CharacterID);
        }


    }
}
