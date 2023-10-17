using UnityEngine;


namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(fileName = "Studio-23", menuName = "Studio-23/Dialogue System/New UI Config")]
    public class DialogueUIConfig : ScriptableObject
    {
        [Header("Text Display Settings")]
        public float letterDelay = 0.05f;
        public float nextSentenceDelayMultiplier = 1.0f;

        [Header("Subtitle Configuration")]
        public Color subtitleColor = Color.black;
        public int subtitleFontSize = 24;
        public bool ShowBackground;
        public Color BackGroundColor = Color.white;

    }

}
