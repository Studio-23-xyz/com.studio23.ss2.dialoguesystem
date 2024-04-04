using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
# if UNITY_EDITOR
namespace Studio23.SS2.DialogueSystem.Utility.EditorLocaleSetter
{
    [InitializeOnLoad]
    public static class EditorLocaleSetter
    {
        static EditorLocaleSetter()
        {
            if (LocalizationSettings.SelectedLocale == null)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.FirstOrDefault(l => l.Identifier.ToString().ToLower().Contains("english") );
                Debug.Log("SELECTED LOCALE SET TO " + (LocalizationSettings.SelectedLocale == null ? "null": LocalizationSettings.SelectedLocale));  
            }
        }
    }
}
#endif