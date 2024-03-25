using Studio23.SS2.DialogueSystem.Data;
using UnityEditor;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

namespace Editor.Data
{
    [CustomEditor(typeof(CharacterData))]
    public class CharacterDataEditor:UnityEditor.Editor
    {
        private string newExpressionName = "QUESTIONABLE EMOJI FACE";
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var charData = target as CharacterData;
            newExpressionName = EditorGUILayout.TextField(newExpressionName);
            if (!string.IsNullOrEmpty(newExpressionName) && GUILayout.Button("Add new  Expression"))
            {
                AddExpression(charData, newExpressionName);
            }
        }

        public static void AddExpression(CharacterData character, string expressionName)
        {
            var expression = ScriptableObject.CreateInstance<CharacterExpressionData>();
            expression.Character = character;
            expression.ExpressionName = expressionName;
            expression.name = expression.GetAssetName();
            character.Expressions.Add(expression);
            
            AssetDatabase.AddObjectToAsset(expression, character);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(character);
        }
    }
}