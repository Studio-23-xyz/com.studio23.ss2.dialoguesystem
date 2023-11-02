using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.DialogueSystem.Editor
{
    public class DialogueSystemWizard : EditorWindow
    {
        private int _currentTab;

        private DialogueGraphEditorUI _dialogueGraphEditorUI;

        private Texture _header;

        [MenuItem("Studio-23/Dialogue System/Dialogue System Wizard")]
        public static void ShowWindow()
        {
            GetWindow<DialogueSystemWizard>("Dialogue System Wizard");
        }

        private void OnEnable()
        {
            _dialogueGraphEditorUI = new DialogueGraphEditorUI();
            _header = Resources.Load<Texture>("DialogueSystemHeader");
        }

        private void OnGUI()
        {

            GUILayout.Box(_header, GUILayout.Height(200), GUILayout.ExpandWidth(true));
            _dialogueGraphEditorUI.ShowWindow();
           
        }


    }
}

