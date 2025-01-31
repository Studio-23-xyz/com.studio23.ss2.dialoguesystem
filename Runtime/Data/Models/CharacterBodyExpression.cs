using System;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Dialogue System/Character Body Expression", fileName = "Character Body Expression")]
    [Serializable]
    public class CharacterBodyExpression : ScriptableObject
    {
        public string ExpressionName;
    }
}
