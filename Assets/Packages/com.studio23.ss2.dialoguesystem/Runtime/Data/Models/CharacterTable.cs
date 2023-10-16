using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.Data
{
    //todo remove this
    [CreateAssetMenu(menuName = "Studio-23/Dialogue System/New Character Table")]
    public class CharacterTable : ScriptableObject
    {
        [Serializable]
        public class CharacterInfo
        {
            public string CharacterID;
            public string CharacterName;
            public List<CharacterReactionImage> Reactions;
        }

        [Serializable]
        public class CharacterReactionImage
        {
            public string Reaction;
            public Sprite Image;
        }

        public List<CharacterInfo> characterList = new List<CharacterInfo>();

        public CharacterInfo GetCharacterInfo(string characterID)
        {
            return characterList.FirstOrDefault<CharacterInfo>(r => r.CharacterID == characterID);
        }
    }

}