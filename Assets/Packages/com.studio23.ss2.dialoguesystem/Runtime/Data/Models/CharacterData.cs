using System;
using System.Collections.Generic;
using UnityEngine;


namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Dialogue System/New Character Data")]
    [Serializable]
    public class CharacterData : ScriptableObject
    {
        public string CharacterID;
        public string CharacterName;
        public Color DialogueColor;
        public List<CharacterExpressionImage> ExpressionTable;
    }

    [Serializable]
    public class CharacterExpressionImage
    {
        public string Reaction;
        public Sprite Image;
    }
}