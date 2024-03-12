using UnityEditor;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

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
            EditorUtility.SetDirty(table.SharedData);
            EditorUtility.SetDirty(table);
        }
    }
}