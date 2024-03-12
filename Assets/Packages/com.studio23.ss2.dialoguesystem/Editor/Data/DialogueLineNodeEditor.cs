using Studio23.SS2.DialogueSystem.Utility;
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
        private string _prevText;

        public override void OnBodyGUI()
        {
            var dialogueLineNode = target as DialogueLineNodeBase;
            
            var collection = LocalizationEditorSettings.GetStringTableCollection(dialogueLineNode.DialogueLocalizedString.TableReference);
            var englishTable = collection.GetTable(DEFAULT_LOCALE) as StringTable;
            var entry = englishTable.GetEntry(dialogueLineNode.DialogueLocalizedString.TableEntryReference.KeyId);
            if (entry == null)
            {
                _prevText = EditorGUILayout.TextArea(_prevText);
                if(string.IsNullOrEmpty(_prevText))
                {
                    EditorGUILayout.HelpBox("Write something", MessageType.Warning);
                }
                else
                {
                    if (GUILayout.Button("Create localized line"))
                    {
                        var key = collection.SharedData.KeyGenerator.GetNextKey();
                        englishTable.AddEntry(key, _prevText);
                        Debug.Log($"new dialogue line entry {_prevText}");
                        englishTable.SaveChanges();
                    }
                }
                
            }
            else
            {
                _prevText = entry.Value;
                var text = EditorGUILayout.TextArea(_prevText);
                if (_prevText != text)
                {
                    EditorGUILayout.HelpBox("Localized string doesn't match. PLEASE SAVE", MessageType.Warning);
                    if (GUILayout.Button("SAVE"))
                    {
                        Debug.Log($"{_prevText} -> {text}");
                        englishTable.SetEntry(dialogueLineNode.DialogueLocalizedString, text);
                        englishTable.SaveChanges();
                    }

                    // prevText = e.Value
                    // dialogueLineNode.DialogueLocalizedString.se
                    // dialogueLineNode.DialogueLocalizedString
                }
            }
            
            // Update serialized object's representation
            serializedObject.Update();
            base.OnBodyGUI();
        }
        
    }
}
