using Studio23.SS2.DialogueSystem.Data;
using Studio23.SS2.DialogueSystem.Utility;
using UnityEditor;

namespace Samples.DialogueChoiceDemo.Editor
{
    [CustomEditor(typeof(DialogueTimelineClip))]
    public class DialogueTimelineClipEditor:UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var clip = target as DialogueTimelineClip;
            var node = clip.Node;
            if (node is DialogueLineNodeBase lineNodeBase)
            {
                EditorGUILayout.HelpBox(node.DialogueLocalizedString.GetLocalizedStringInEditor(), MessageType.Info);
            }
        }
    }
}