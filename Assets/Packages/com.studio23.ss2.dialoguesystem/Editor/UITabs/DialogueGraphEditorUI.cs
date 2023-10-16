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

        private string folderPath = $"Assets/Resources/DialogueSystem/Graphs/";


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
            if (GUILayout.Button("Open CSV File"))
            {
                _csvFilePath = EditorUtility.OpenFilePanel("Open CSV File", "", "csv");

            }

            if (!string.IsNullOrEmpty(_csvFilePath) && GUILayout.Button("Populate Dialogue Graph"))
            {
                PopulateDialogueGraph();
            }
        }

        private void CreateNewDialogueGraph()
        {
            _dialogueGraph = ScriptableObject.CreateInstance<DialogueGraph>();
            _dialogueGraph.SkippableDialogue = true;

            if (!Directory.Exists($"{folderPath}/{GraphName}"))
            {
                Directory.CreateDirectory($"{folderPath}/{GraphName}");
            }
            AssetDatabase.CreateAsset(_dialogueGraph, $"{folderPath}/{GraphName}/{GraphName}.asset");
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

            string nodeDirectory = $"{folderPath}/{GraphName}/Nodes";
            if (!Directory.Exists(nodeDirectory))
            {
                Directory.CreateDirectory(nodeDirectory);
            }


            AssetDatabase.CreateAsset(node, $"{nodeDirectory}/{nodeName}.asset");
            PopulateDialogueNodeValues(node, values);
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
            node.CharacterInfo = _characterTable.GetCharacterInfo(node.CharacterID);
        }


    }
}

