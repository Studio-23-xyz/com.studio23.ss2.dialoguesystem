using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.Editor
{
    public class DialogueExpressionTableGeneratorWindow : EditorWindow
    {
        private class Property
        {
            public string name;
            public bool isEditing;
        }

        private List<Property> properties = new List<Property>();
        private static string _className = "DialogueExpressionsTable";
        private static string _nameSpace = "Studio23.SS2.DialogueSystem.Data";
        private Vector2 scrollPosition;

        private string _lastExpressionName = "Questionable emoji face";


        [MenuItem("Studio-23/DialogueSystem/Expression Table Generator")]
        public static void OpenWindow()
        {
            DialogueExpressionTableGeneratorWindow window = GetWindow<DialogueExpressionTableGeneratorWindow>("Dialogue Expression Table Generator");
            window.Initialize();

        }

        public void Initialize()
        {
            LoadExpressionsTable();
            this.minSize = new Vector2(250, 150);
        }

        private void LoadExpressionsTable()
        {
            FetchExpressionsList();
            properties = CurrentExpressionsList.Select(e => new Property(){ name = e }).ToList();
        }

        private void OnGUI()
        {
            GUILayout.Label("Dialogue Expression Table Generator", EditorStyles.boldLabel);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < properties.Count; i++)
            {
                GUILayout.BeginHorizontal();

                if (properties[i].isEditing)
                {
                    properties[i].name = EditorGUILayout.TextField(properties[i].name);
                }
                else
                {
                    GUILayout.Label(properties[i].name, EditorStyles.label);
                }

                if (GUILayout.Button("Edit"))
                {
                    properties[i].isEditing = !properties[i].isEditing;
                }

                if (GUILayout.Button("Remove"))
                {
                    properties.RemoveAt(i);
                }

                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            _lastExpressionName = EditorGUILayout.TextField("Expression To Add",_lastExpressionName);
            if (GUILayout.Button("Add Expression"))
            {
                properties.Add(new Property() { name = _lastExpressionName });
            }

            if (GUILayout.Button("Generate"))
            {
                GenerateStringProperties();
            }
            
            if (GUILayout.Button("Refresh"))
            {
                LoadExpressionsTable();
            }
        }

        private void GenerateStringProperties()
        {
            string scriptContent = $"namespace {_nameSpace}\n{{\n";

            scriptContent += $"\tpublic static class {_className}\n\t{{\n";
            
            foreach (var property in properties)
            {
                
                if (!string.IsNullOrEmpty(property.name))
                {
                    var propName = property.name.ToUpper().Replace(" ", "_");
                    scriptContent += $"\t\tpublic static readonly string {propName} = \"{property.name}\";\n";
                }
            }

            scriptContent += "\t}\n";
            scriptContent += "}";

            string scriptDirectory = Path.Combine("Assets", "Resources");
            string scriptPath = Path.Combine(scriptDirectory, $"{_className}.cs");

            if (!Directory.Exists(scriptDirectory))
            {
                Directory.CreateDirectory(scriptDirectory);
            }
            File.WriteAllText(scriptPath, scriptContent);
            AssetDatabase.Refresh();
        }

        public static List<string> FetchExpressionsList()
        {
            CurrentExpressionsList = GetExpressionsTable();
            return CurrentExpressionsList;
        }

        public static List<string> CurrentExpressionsList { get; set; }

        static List<string> GetExpressionsTable()
        {
            List<string> list = new List<string>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type achievementsTableType = assembly.GetType("Studio23.SS2.DialogueSystem.Data.DialogueExpressionsTable");

                if (achievementsTableType != null)
                {
                    FieldInfo[] fields = achievementsTableType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                    foreach (FieldInfo field in fields)
                    {
                        if (field.FieldType == typeof(string))
                        {
                            list.Add(field.Name);
                        }
                    }

                    Debug.Log("AchievementsTable.cs properties loaded, default keys created.");
                    return list;
                }
            }
            Debug.LogError("AchievementsTable.cs class not found.");
            return list;
        }
    }
}



