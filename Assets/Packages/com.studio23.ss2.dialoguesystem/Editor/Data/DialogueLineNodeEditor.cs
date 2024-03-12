using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using XNodeEditor;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CustomNodeEditor(typeof(DialogueLineNodeBase))]
    public class DialogueLineNodeEditor : NodeEditor
    {

        public static string DEFAULT_LOCALE = "en";

        public override void OnBodyGUI()
        {
            var dialogueLineNode = target as DialogueLineNodeBase;
            
            var collection = LocalizationEditorSettings.GetStringTableCollection(dialogueLineNode.DialogueLocalizedString.TableReference);
            var englishTable = collection.GetTable(DEFAULT_LOCALE) as StringTable;
            var entry = englishTable.GetEntry(dialogueLineNode.DialogueLocalizedString.TableEntryReference.KeyId);

            var prevText = entry.Value;
            var text = EditorGUILayout.TextArea(prevText);
            if (prevText != text)
            {
                Debug.Log($"{prevText} -> {text}");
                englishTable.AddEntry(dialogueLineNode.DialogueLocalizedString.TableEntryReference.KeyId, text);
                // prevText = e.Value
                // dialogueLineNode.DialogueLocalizedString.se
                // dialogueLineNode.DialogueLocalizedString
            }
            // Update serialized object's representation
            serializedObject.Update();
            base.OnBodyGUI();
        }
        
    }
}
