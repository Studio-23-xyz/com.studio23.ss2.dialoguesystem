using Studio23.SS2.DialogueSystem.Utility;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using XNodeEditor;

namespace Studio23.SS2.DialogueSystem.Data
{
    [CustomNodeEditor(typeof(DialogueLineNodeBase))]
    public class DialogueLineNodeEditor : NodeEditor
    {
        public static string DEFAULT_LOCALE = "en";
        private string _text;

        public override void OnBodyGUI()
        {
            // Update serialized object's representation
            serializedObject.Update();
            var dialogueLineNode = target as DialogueLineNodeBase;
            DrawLocalizationStringHelperGUI(dialogueLineNode);

            base.OnBodyGUI();
        }

        private void DrawLocalizationStringHelperGUI(DialogueLineNodeBase dialogueLineNode)
        {
            var collection = LocalizationEditorSettings.GetStringTableCollection(dialogueLineNode.DialogueLocalizedString.TableReference);
            var defaultLocaleTable = collection.GetTable(DEFAULT_LOCALE) as StringTable;
            if (defaultLocaleTable == null)
            {
                EditorGUILayout.HelpBox("No language Table", MessageType.Error);
                return;
            }
            DrawCustomDialogueTextbox(defaultLocaleTable, dialogueLineNode, collection);
            base.OnBodyGUI();
        }

        private void DrawCustomDialogueTextbox(StringTable defaultTable, DialogueLineNodeBase dialogueLineNode,
            StringTableCollection collection)
        {
            var entry = defaultTable.GetEntry(dialogueLineNode.DialogueLocalizedString.TableEntryReference.KeyId);
            if (entry == null)
            {
                _text = EditorGUILayout.TextArea(_text);
                
                if(string.IsNullOrEmpty(_text))
                {
                    EditorGUILayout.HelpBox("Write something", MessageType.Warning);
                }
                else
                {
                    if (GUILayout.Button("Create localized line"))
                    {
                        CreateNewEntry(collection, defaultTable, dialogueLineNode);
                    }
                }
            }
            else
            {
                var localizedText = entry.Value;
                if (string.IsNullOrEmpty(_text))
                {
                    _text = localizedText;
                }
                _text = EditorGUILayout.TextArea(_text);
                if (_text != localizedText)
                {
                    EditorGUILayout.HelpBox("Localized string doesn't match. PLEASE SAVE", MessageType.Warning);
                    if (GUILayout.Button("SAVE"))
                    {
                        Debug.Log($"{localizedText} -> {_text}");
                        defaultTable.SetEntry(dialogueLineNode.DialogueLocalizedString, _text);
                        defaultTable.SaveChanges();
                        dialogueLineNode.DialogueLocalizedString.RefreshString();
                        EditorUtility.SetDirty(dialogueLineNode);
                    }else if (GUILayout.Button("RESET"))
                    {
                        _text = localizedText;
                    }
                }
                if (GUILayout.Button("Replace with new Entry"))
                {
                    CreateNewEntry(collection, defaultTable, dialogueLineNode);
                }
            }
        }

        private void CreateNewEntry(StringTableCollection collection, StringTable englishTable,
            DialogueLineNodeBase dialogueLineNode)
        {
            StringTableEntry entry;
            var key = _text.Substring(0, Mathf.Min(10, _text.Length));
            collection.SharedData.AddKey(key);
            entry = englishTable.AddEntry(key, _text);
                        
            Debug.Log($"new dialogue line entry {_text}");
                        
            englishTable.SaveChanges();
                        
            dialogueLineNode.DialogueLocalizedString =
                new LocalizedString(collection.SharedData.TableCollectionNameGuid, entry.KeyId);
            EditorUtility.SetDirty(dialogueLineNode);
        }
    }
}
