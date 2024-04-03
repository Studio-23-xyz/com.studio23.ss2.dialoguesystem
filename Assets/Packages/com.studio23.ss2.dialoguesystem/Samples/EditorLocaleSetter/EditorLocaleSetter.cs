using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Samples.EditorLocaleSetter.Editor
{
    [InitializeOnLoad]
    public static class EditorLocaleSetter
    {
        static EditorLocaleSetter()
        {
            if (LocalizationSettings.SelectedLocale == null)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.FirstOrDefault(l => l.Identifier.ToString().ToLower().Contains("english") );
                // Locale l = null;
                // foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
                // {
                //     if (locale.Identifier.ToString().ToLower().Contains("english"))
                //     {
                //         l = locale;
                //         Debug.Log(locale.Identifier + " aaa " );
                //     }
                // }
                Debug.Log("SELECTED LOCALE SET TO " + (LocalizationSettings.SelectedLocale == null ? "null": LocalizationSettings.SelectedLocale));  
            }
        }
    }
}