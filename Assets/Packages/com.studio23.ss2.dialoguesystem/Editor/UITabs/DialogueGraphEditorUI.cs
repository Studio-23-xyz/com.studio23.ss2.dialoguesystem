using Studio23.SS2.DialogueSystem.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.Editor
{
    public class DialogueGraphEditorUI
    {
        private string _csvFilePath;
        private CharacterTable _characterTable;
        private DialogueGraph _dialogueGraph;

        public string GraphName;

        private string _dialogueSystemResourcesFolder => $"Assets/Resources/DialogueSystem";
        private string _dialogueGraphFolderPath => $"{_dialogueSystemResourcesFolder}/Graphs/";

        #region GUI
        public void ShowWindow()
        {
            DrawGUI();
        }

        private void DrawGUI()
        {

            _characterTable = Resources.Load<CharacterTable>("DialogueSystem/CharacterTable");
            if (_characterTable == null)
            {
                EditorGUILayout.HelpBox("Character Table not Found! Create One First", MessageType.Error);

                if (GUILayout.Button("Create New Character Table"))
                {
                    CreateNewCharacterTable();
                }

                return;
            }

            if (!_characterTable.IsValid)
            {
                EditorGUILayout.HelpBox("Character Table invalid please check!", MessageType.Warning);
                return;
            }


            GUILayout.Label("Create New Dialogue Graph", EditorStyles.boldLabel);

            _dialogueGraph = (DialogueGraph)EditorGUILayout.ObjectField("Dialogue Graph", _dialogueGraph, typeof(DialogueGraph), false);

            if (_dialogueGraph == null)
            {

                if (string.IsNullOrEmpty(GraphName))
                {
                    GraphName = "New Graph";
                }

                GraphName = EditorGUILayout.TextField("Graph Name", GraphName);

                if (GUILayout.Button("Create New Dialogue Graph"))
                {
                    CreateNewDialogueGraph();
                }
            }



            GUILayout.Space(20);

            GUILayout.Label("Open CSV File and Populate Dialogue Graph", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Browse"))
            {
                _csvFilePath = EditorUtility.OpenFilePanel("Open CSV File", "", "csv");
            }

            if (GUILayout.Button("Get Template"))
            {

                string templatePath = EditorUtility.SaveFilePanel("Save CSV File", "", "Dialogue Template", "csv");
                using (StreamWriter writer = new StreamWriter(templatePath))
                {
                    writer.WriteLine("ID,Character Reaction,Dialogue Text,FMOD Event Path");
                }
            }

            GUILayout.EndHorizontal();


            if (!string.IsNullOrEmpty(_csvFilePath) && GUILayout.Button("Populate Dialogue Graph"))
            {
                PopulateDialogueGraph();
            }
        }
        #endregion



        private void CreateNewCharacterTable()
        {
            _characterTable = ScriptableObject.CreateInstance<CharacterTable>();

            if (!Directory.Exists(_dialogueSystemResourcesFolder))
            {
                Directory.CreateDirectory(_dialogueSystemResourcesFolder);
            }
            AssetDatabase.CreateAsset(_characterTable, $"{_dialogueSystemResourcesFolder}/CharacterTable.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        #region GraphCreation
        private void CreateNewDialogueGraph()
        {
            _dialogueGraph = ScriptableObject.CreateInstance<DialogueGraph>();

            if (!Directory.Exists($"{_dialogueGraphFolderPath}/{GraphName}"))
            {
                Directory.CreateDirectory($"{_dialogueGraphFolderPath}/{GraphName}");
            }
            AssetDatabase.CreateAsset(_dialogueGraph, $"{_dialogueGraphFolderPath}/{GraphName}/{GraphName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void PopulateDialogueGraph()
        {
            if (_dialogueGraph == null)
            {
                Debug.LogError("No Dialogue Graph selected.");
                return;
            }


            string[] lines = File.ReadAllText(_csvFilePath).Split('\n');


            DialogueBase node = GetNewDialogueNode(ScriptableObject.CreateInstance<StartNode>(), 1.ToString(), lines[1].Split(','));
        
            _dialogueGraph.AddNewNode(node);


            for (int i = 2; i < lines.Length - 2; i++)
            {
                string[] values = lines[i].Split(',');
                if (values.Length >= 4)
                {

                    node = GetNewDialogueNode(ScriptableObject.CreateInstance<DialogueNode>(), i.ToString(), values);
                 
                    _dialogueGraph.AddNewNode(node);
                }
                else
                {
                    //TODO Show error
                }
            }

            node = GetNewDialogueNode(ScriptableObject.CreateInstance<EndNode>(), (lines.Length - 2).ToString(), lines[lines.Length - 2].Split(','));
            _dialogueGraph.AddNewNode(node);


            EditorUtility.SetDirty(_dialogueGraph);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private DialogueBase GetNewDialogueNode(DialogueBase node, string nodeName, string[] values)
        {

            string nodeDirectory = $"{_dialogueGraphFolderPath}/{GraphName}/Nodes";
            if (!Directory.Exists(nodeDirectory))
            {
                Directory.CreateDirectory(nodeDirectory);
            }


            AssetDatabase.CreateAsset(node, $"{nodeDirectory}/{nodeName}.asset");
            PopulateDialogueNodeValues(node, values);
            EditorUtility.SetDirty(node);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return node;
        }

        private void PopulateDialogueNodeValues(DialogueBase node, string[] values)
        {
            node.CharacterID = values[0];
            node.CharacterReaction = values[1];
            node.DialogueText = values[2];
            node.FMODEventPath = values[3];
        }

        #endregion
    }

}

