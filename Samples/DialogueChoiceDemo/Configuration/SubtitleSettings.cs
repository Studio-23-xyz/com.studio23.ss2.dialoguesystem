using UnityEngine;
[CreateAssetMenu(fileName = "SubtitleSettings", menuName = "Studio-23/Dialogue System/Subtitle Settings")]
public class SubtitleSettings : ScriptableObject
{
    public Color SubtitleColor = Color.black;
    public int SubtitleFontSize = 24;
    public bool DropShadow = false;

    public bool EnableCharacterColor = true;
    public bool ShowBackground;
    public Color BackGroundColor = Color.white;
}