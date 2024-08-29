using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Dialogue System/New Character Data")]
    [Serializable]
    public class CharacterData : ScriptableObject
    {
        public LocalizedString CharacterName;
        public Sprite DefaultSprite;
        public Color DialogueColor;
        public List<CharacterExpressionData> Expressions;

        public CharacterExpressionData GetExpressionByName(string expressionName)
        {
            return Expressions.FirstOrDefault(e => e.ExpressionName == expressionName);
        }
        
        public int GetExpressionIndexByName(CharacterExpressionData expressionData)
        {
            return Expressions.IndexOf(expressionData);
        }
    }
}