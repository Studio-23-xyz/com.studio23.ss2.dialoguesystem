using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Dialogue System/New Character Table")]
    public class CharacterTable : ScriptableObject
    {
        public bool IsValid
        {
            get
            {
                if (characterList == null || characterList.Count == 0)
                    return false;

                // Create a set to keep track of unique Character IDs
                HashSet<string> uniqueCharacterIDs = new HashSet<string>();

                foreach (var characterData in characterList)
                {
                    if (string.IsNullOrEmpty(characterData.CharacterID) || string.IsNullOrEmpty(characterData.CharacterName))
                        return false;

                    // Check if the Character ID is already in the set
                    if (uniqueCharacterIDs.Contains(characterData.CharacterID))
                        return false;

                    // Add the Character ID to the set
                    uniqueCharacterIDs.Add(characterData.CharacterID);
                }

                return true; // All checks passed, the CharacterTable is valid
            }
        }

        public List<CharacterData> characterList = new List<CharacterData>();

        public CharacterData GetCharacterData(string characterID)
        {
            return characterList.FirstOrDefault<CharacterData>(r => r.CharacterID == characterID);
        }
    }

}