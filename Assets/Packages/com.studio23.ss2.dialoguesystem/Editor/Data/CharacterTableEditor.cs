using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Studio23.SS2.DialogueSystem.Data;

namespace com.studio23.ss2.dialoguesystem.editor
{
    public class CharacterTableEditorWindow : EditorWindow
    {
        private CharacterTable characterTable;
        private string characterID = "";
        private string characterName = "";
        private string csvFilePath = "";

        [MenuItem("Studio-23/Dialogue System/Character Table")]
        public static void ShowWindow()
        {
            CharacterTableEditorWindow window = GetWindow<CharacterTableEditorWindow>("Character Table");
            window.LoadCharacterTable();
        }

        private void OnGUI()
        {
            if (characterTable != null)
            {
                GUILayout.Label("Character Table", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                characterID = EditorGUILayout.TextField(characterID);
                characterName = EditorGUILayout.TextField(characterName);
                if(string.IsNullOrEmpty(characterName) || string.IsNullOrEmpty(characterID))
                {
                    characterID = "New Character ID";
                    characterName = "New Character Name";
                }


                if (GUILayout.Button("Add") && !string.IsNullOrEmpty(characterID) && !string.IsNullOrEmpty(characterName) && characterTable != null)
                {
                    if (IsCharacterIDUnique(characterID))
                    {
                        characterTable.characterList.Add(new CharacterTable.CharacterInfo() { characterID = characterID, characterName = characterName });
                        EditorUtility.SetDirty(characterTable);
                        characterID = "New Character ID";
                        characterName = "New Character Name";
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Duplicate Character ID", "Character ID must be unique.", "OK");
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.LabelField("Character Count: " + characterTable.characterList.Count);

                for (int i = 0; i < characterTable.characterList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    characterTable.characterList[i].characterID = EditorGUILayout.TextField(characterTable.characterList[i].characterID);
                    characterTable.characterList[i].characterName = EditorGUILayout.TextField(characterTable.characterList[i].characterName);
                    if (GUILayout.Button("Remove", GUILayout.MaxWidth(80)))
                    {
                        characterTable.characterList.RemoveAt(i);
                        GUIUtility.ExitGUI();
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(characterTable);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Character Table not found. Click 'Add Character Table' to create one.", MessageType.Warning);

                if (GUILayout.Button("Create New Character Table"))
                {
                    CreateCharacterTable();
                }

                // Add a Browse File button for selecting the input CSV file
                if (GUILayout.Button("From CSV"))
                {
                    csvFilePath = EditorUtility.OpenFilePanel("Select CSV File", "", "csv");
                    ReplaceCharacterTableFromCSVTemplate(csvFilePath);
                }

                // Add a Get Template button for saving CharacterTable as a CSV file
                if (GUILayout.Button("CSV Template"))
                {
                    string savePath = EditorUtility.SaveFilePanel("Save CSV File", "", "CharacterTableTemplate", "csv");
                    if (!string.IsNullOrEmpty(savePath))
                    {
                        SaveCharacterTableToCSV(characterTable, savePath);
                    }
                }

            }

           
        }

        private void LoadCharacterTable()
        {
            characterTable = Resources.Load<CharacterTable>("DialogueSystem/CharacterTable");

        }

        private void CreateCharacterTable()
        {
            
            if (characterTable == null)
            {
                characterTable = ScriptableObject.CreateInstance<CharacterTable>();
                characterTable.characterList = new List<CharacterTable.CharacterInfo>();

                string folderPath = Application.dataPath + "/Resources/DialogueSystem";
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                AssetDatabase.CreateAsset(characterTable, "Assets/Resources/DialogueSystem/CharacterTable.asset");
                AssetDatabase.SaveAssets();
            }
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
                            newCharacterList.Add(new CharacterTable.CharacterInfo() { characterID = characterID, characterName = characterName });
                        }
                    }
                }
                CreateCharacterTable();
                // Update the character table with the new data
                characterTable.characterList = newCharacterList;

                // Mark the asset as dirty and save it
                EditorUtility.SetDirty(characterTable);
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
            return characterTable.characterList.All(c => c.characterID != id);
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
