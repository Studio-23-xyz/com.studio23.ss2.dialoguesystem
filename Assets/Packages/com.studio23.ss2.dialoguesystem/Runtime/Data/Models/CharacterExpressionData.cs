using System;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.Data
{
    [Serializable]
    public class CharacterExpressionData:ScriptableObject
    {
        public CharacterData Character;
        public string ExpressionName;
        public Sprite Image;

        public string GetAssetName()
        {
            return $"{Character.name}_{ExpressionName}" ;
        }
    }
}