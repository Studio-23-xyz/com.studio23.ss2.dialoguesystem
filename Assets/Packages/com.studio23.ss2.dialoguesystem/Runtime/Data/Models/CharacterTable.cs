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

        public bool IsValid
        {
            get
            {
                // Check if characterList is null or empty
                if (characterList == null || characterList.Count == 0)
                    return false;

                // Check if any characterInfo in characterList has empty CharacterID or CharacterName
                foreach (var characterInfo in characterList)
                {
                    if (string.IsNullOrEmpty(characterInfo.CharacterID) || string.IsNullOrEmpty(characterInfo.CharacterName))
                        return false;
                }

                return true; // All checks passed, the CharacterTable is valid
            }
        }



        public List<CharacterInfo> characterList = new List<CharacterInfo>();

        public CharacterInfo GetCharacterInfo(string characterID)
        {
            return characterList.FirstOrDefault<CharacterInfo>(r => r.CharacterID == characterID);
        }
    }

}