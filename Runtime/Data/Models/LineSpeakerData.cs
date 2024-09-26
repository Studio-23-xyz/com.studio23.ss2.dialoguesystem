using System;
using UnityEngine.Serialization;

namespace Studio23.SS2.DialogueSystem.Data
{
    [Serializable]
    public class LineSpeakerData
    {
        public CharacterData Character;
        public CharacterExpressionData Expression;
    }
}