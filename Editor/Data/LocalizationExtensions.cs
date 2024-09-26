using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

# if UNITY_EDITOR
using UnityEditor;
# endif

namespace Studio23.SS2.DialogueSystem.Utility
{
    public static class LocalizationExtensions
    {
        public static void SetEntry(this StringTable table, LocalizedString localizedString, string newText)
        {
            table.AddEntry(localizedString.TableEntryReference.KeyId, newText);
        }
        
        public static void SaveChanges(this StringTable table)
        {
# if UNITY_EDITOR
            EditorUtility.SetDirty(table.SharedData);
            EditorUtility.SetDirty(table);
# endif
        }
        
# if UNITY_EDITOR
        /// <summary>
        /// Use this to get localized string value in editor
        /// for whatever reason notmal getString doesn't work
        /// </summary>
        /// <param name="localizedString"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static string GetLocalizedStringInEditor(this LocalizedString localizedString, string locale = "en")
        {
            var collection = LocalizationEditorSettings.GetStringTableCollection(localizedString.TableReference);
            var englishTable = collection.GetTable(locale) as StringTable;
            var entry = englishTable.GetEntry(localizedString.TableEntryReference.KeyId);
            if (entry == null)
            {
                return null;
            }

            return entry.Value;
        }
# endif
    }
}