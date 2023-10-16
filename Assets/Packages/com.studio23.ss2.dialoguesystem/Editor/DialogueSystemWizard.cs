using Studio23.SS2.DialogueSystem.Editor;
using UnityEditor;
using UnityEngine;

public class DialogueSystemWizard : EditorWindow
{
    private int _currentTab;

    private CharacterTableEditorUI _characterTableWizard;
    private DialogueGraphEditorUI _dialogueGraphEditorUI;

    private Texture _header;

    [MenuItem("Studio-23/Dialogue System/Dialogue System Wizard")]
    public static void ShowWindow()
    {
        GetWindow<DialogueSystemWizard>("Dialogue System Wizard");
    }

    private void OnEnable()
    {
        _characterTableWizard = new CharacterTableEditorUI();
        _dialogueGraphEditorUI = new DialogueGraphEditorUI();
        _header = Resources.Load<Texture>("Header");
    }

    private void OnGUI()
    {

        GUILayout.Box(_header, GUILayout.Height(200), GUILayout.ExpandWidth(true));

        _currentTab = GUILayout.Toolbar(_currentTab, new string[] { "Character Table", "Dialogue Graph" });

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        if (_currentTab == 0)
        {
            _characterTableWizard.ShowWindow();
        }
        else if (_currentTab == 1)
        {
            _dialogueGraphEditorUI.ShowWindow();
        }

        EditorGUILayout.EndVertical();
    }


}
