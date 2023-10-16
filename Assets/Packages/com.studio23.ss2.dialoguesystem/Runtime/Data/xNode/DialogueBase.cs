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
        public CharacterTable.CharacterInfo CharacterInfo;


        [TextArea(3, 10)]
        public string DialogueText;
        public string FMODEventPath;




    }
}
