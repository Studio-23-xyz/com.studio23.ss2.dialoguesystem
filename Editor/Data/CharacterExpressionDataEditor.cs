using Studio23.SS2.DialogueSystem.Data;
using UnityEditor;
using UnityEngine;

namespace Editor.Data
{
    [CustomEditor(typeof(CharacterExpressionData))]
    public class CharacterExpressionDataEditor:UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var expressionData = target as CharacterExpressionData;
            if (GUILayout.Button("RENAME"))
            {
                Rename(expressionData);
            }
            
            if (GUILayout.Button("REMOVE"))
            {
                RemoveExpression(expressionData);
            }
        }

        public void RemoveExpression(CharacterExpressionData data)
        {
            data.Character.Expressions.Remove(data);
            Undo.DestroyObjectImmediate(data);
            AssetDatabase.SaveAssets();
        }

        public void Rename(CharacterExpressionData data)
        {
            data.name = data.GetAssetName();
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(data);
        }
    }
}