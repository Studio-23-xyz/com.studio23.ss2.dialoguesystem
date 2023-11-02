using UnityEngine;


namespace Studio23.SS2.DialogueSystem.Data
{
    [CreateAssetMenu(fileName = "SubtitleSettings", menuName = "Studio-23/Dialogue System/Subtitile Settings")]
    public class SubtitileSettings : ScriptableObject
    {

        public Color subtitleColor = Color.black;
        public int subtitleFontSize = 24;
        public bool dropShadow = false;


        public bool EnableCharacterColor = true;
        public bool ShowBackground;
        public Color BackGroundColor = Color.white;

    }

}
