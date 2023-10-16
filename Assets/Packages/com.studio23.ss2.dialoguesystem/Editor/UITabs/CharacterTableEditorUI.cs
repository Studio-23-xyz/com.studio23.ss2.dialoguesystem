using Studio23.SS2.DialogueSystem.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.Editor
{
    public class CharacterTableEditorUI
    {
        private CharacterTable _characterTable;
        private string _characterID;
        private string _characterName;
        private string _csvFilePath;


        public void ShowWindow()
        {

            DrawGUI();
            LoadCharacterTable();
        }

        private void DrawGUI()
        {
            if (_characterTable != null)
            {
                GUILayout.Label("Character Table", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                _characterID = EditorGUILayout.TextField(_characterID);
                _characterName = EditorGUILayout.TextField(_characterName);
                if (string.IsNullOrEmpty(_characterName) || string.IsNullOrEmpty(_characterID))
                {
                    _characterID = "New Character ID";
                    _characterName = "New Character Name";
                }


                if (GUILayout.Button("Add") && !string.IsNullOrEmpty(_characterID) && !string.IsNullOrEmpty(_characterName) && _characterTable != null)
                {
                    if (IsCharacterIDUnique(_characterID))
                    {
                        _characterTable.characterList.Add(new CharacterTable.CharacterInfo() { CharacterID = _characterID, CharacterName = _characterName });
                        EditorUtility.SetDirty(_characterTable);
                        _characterID = "New Character ID";
                        _characterName = "New Character Name";
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Duplicate Character ID", "Character ID must be unique.", "OK");
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.LabelField("Character Count: " + _characterTable.characterList.Count);

                for (int i = 0; i < _characterTable.characterList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    _characterTable.characterList[i].CharacterID = EditorGUILayout.TextField(_characterTable.characterList[i].CharacterID);
                    _characterTable.characterList[i].CharacterName = EditorGUILayout.TextField(_characterTable.characterList[i].CharacterName);
                    if (GUILayout.Button("Remove", GUILayout.MaxWidth(80)))
                    {
                        _characterTable.characterList.RemoveAt(i);
                        GUIUtility.ExitGUI();
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(_characterTable);
                }

                if (GUILayout.Button("Delete Table"))
                {
                    DeleteCharacterTable();
                }


            }
            else
            {
                EditorGUILayout.HelpBox("Character Table not found. Click 'Add Character Table' to create one.", MessageType.Error);

                if (GUILayout.Button("Create New Character Table"))
                {
                    CreateCharacterTable();
                }

                // Add a Browse File button for selecting the input CSV file
                if (GUILayout.Button("From CSV"))
                {
                    _csvFilePath = EditorUtility.OpenFilePanel("Select CSV File", "", "csv");
                    ReplaceCharacterTableFromCSVTemplate(_csvFilePath);
                }

                // Add a Get Template button for saving CharacterTable as a CSV file
                if (GUILayout.Button("CSV Template"))
                {
                    string savePath = EditorUtility.SaveFilePanel("Save CSV File", "", "CharacterTableTemplate", "csv");
                    if (!string.IsNullOrEmpty(savePath))
                    {
                        SaveCharacterTableToCSV(_characterTable, savePath);
                    }
                }

            }


        }

        private void LoadCharacterTable()
        {
            _characterTable = Resources.Load<CharacterTable>("DialogueSystem/CharacterTable");
        }

        private void CreateCharacterTable()
        {

            if (_characterTable == null)
            {
                _characterTable = ScriptableObject.CreateInstance<CharacterTable>();
                _characterTable.characterList = new List<CharacterTable.CharacterInfo>();

                string folderPath = Application.dataPath + "/Resources/DialogueSystem";
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                AssetDatabase.CreateAsset(_characterTable, "Assets/Resources/DialogueSystem/CharacterTable.asset");
                AssetDatabase.SaveAssets();
            }
        }

        private void DeleteCharacterTable()
        {
            AssetDatabase.DeleteAsset("Assets/Resources/DialogueSystem/CharacterTable.asset");
        }

        private void ReplaceCharacterTableFromCSVTemplate(string csvFilePath)
        {
            if (File.Exists(csvFilePath))
            {
                // Load the new character table from the CSV file
                List<CharacterTable.CharacterInfo> newCharacterList = new List<CharacterTable.CharacterInfo>();

                using (StreamReader reader = new StreamReader(csvFilePath))
                {
                    // Skip the header line
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] parts = line.Split(',');

                        if (parts.Length == 2)
                        {
                            string characterID = parts[0];
                            string characterName = parts[1];
                            newCharacterList.Add(new CharacterTable.CharacterInfo() { CharacterID = characterID, CharacterName = characterName });
                        }
                    }
                }
                CreateCharacterTable();
                // Update the character table with the new data
                _characterTable.characterList = newCharacterList;

                // Mark the asset as dirty and save it
                EditorUtility.SetDirty(_characterTable);
                AssetDatabase.SaveAssets();

                // Display a confirmation message
                EditorUtility.DisplayDialog("Character Table Replaced", "The Character Table has been replaced with data from the CSV template.", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("File Not Found", "The selected CSV template file does not exist.", "OK");
            }
        }

        private bool IsCharacterIDUnique(string id)
        {
            return _characterTable.characterList.All(c => c.CharacterID != id);
        }


        private void SaveCharacterTableToCSV(CharacterTable characterTable, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("CharacterID,CharacterName");
            }
        }



    }
}
