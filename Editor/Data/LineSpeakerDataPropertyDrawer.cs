using System.Linq;
using Studio23.SS2.DialogueSystem.Data;
using UnityEditor;
using UnityEngine;

namespace Editor.Data
{
    [CustomPropertyDrawer(typeof(LineSpeakerData))]
    public class LineSpeakerDataPropertyDrawer:PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 3;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            // // Calculate rects
            // var charRect = new Rect(position.x, position.y, 30, position.height);
            // var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
            // var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);
            //
            // // Draw fields - pass GUIContent.none to each so they are drawn without labels
            // EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
            // EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
            // position.y += 30;
            var lineSpeakerData = property.boxedValue as LineSpeakerData;
            // EditorGUI.LabelField(position, lineSpeakerData is LineSpeakerData?"asdas":"zz");
            var charFieldRect = position;
            charFieldRect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(charFieldRect, property.FindPropertyRelative("Character"), new GUIContent("Character"));
            if (lineSpeakerData.Character != null)
            {
                position.y += EditorGUIUtility.singleLineHeight;
                var expressions = lineSpeakerData.Character.Expressions
                    .Where(ced => ced != null)
                    .Select(ced => ced.ExpressionName).ToArray();
                // EditorGUI.LabelField(position," :" + lineSpeakerData);
                if (expressions.Length == 0)
                {
                    EditorGUI.HelpBox(position, "NO EXPRESSIONS", MessageType.Error);                    
                }
                else
                {
                    var expressionProp = property.FindPropertyRelative("Expression");

                    var prevIndex = lineSpeakerData.Character.GetExpressionIndexByName(lineSpeakerData.Expression);
                    var expressionPopupRect = position;
                    expressionPopupRect.height = EditorGUIUtility.singleLineHeight;
                    var expressionIndex = EditorGUI.Popup(expressionPopupRect, prevIndex, expressions);
                    if (expressionIndex != prevIndex)
                    {
                        if (expressionIndex >= 0 && expressionIndex < expressions.Length)
                        {
                            expressionProp.objectReferenceValue = lineSpeakerData.Character.Expressions[expressionIndex];
                            expressionProp.serializedObject.ApplyModifiedProperties();
                            
                        }
                    }
                    position.y += EditorGUIUtility.singleLineHeight;
                    var expressionFieldRect = position;
                    expressionFieldRect.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.ObjectField(expressionFieldRect, expressionProp, GUIContent.none);
                    EditorGUI.EndDisabledGroup();
                }
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}