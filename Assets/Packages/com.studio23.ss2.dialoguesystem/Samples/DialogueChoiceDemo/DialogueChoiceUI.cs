using System;
using System.Collections.Generic;
using Studio23.SS2.DialogueSystem.Core;
using Studio23.SS2.DialogueSystem.Data;
using UnityEngine;


public class DialogueChoiceUI:MonoBehaviour
{
    [SerializeField] private List<DialogueChoiceNodeBase> _sortedChoices;
    [SerializeField] private Transform _buttonContainer;
    [SerializeField] private DialogueChoiceButton _buttonPrefab;
    [SerializeField] List<DialogueChoiceButton> _spawnedButtons;

    private void Awake()
    {
        _buttonContainer.gameObject.SetActive(false);
    }

    private void Start()
    {
        DialogueSystem.Instance.OnDialogueChoiceStarted += HandleDialogueChoiceStarted;
        DialogueSystem.Instance.OnDialogueChoiceEnded += HandleDialogueChoiceEnded;
    }

    private void OnDestroy()
    {
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.OnDialogueChoiceStarted -= HandleDialogueChoiceStarted;
            DialogueSystem.Instance.OnDialogueChoiceEnded -= HandleDialogueChoiceEnded;
        }
    }

    private void HandleDialogueChoiceEnded(DialogueChoicesNode choicesNode)
    {
        _buttonContainer.gameObject.SetActive(false);
    }

    private void HandleDialogueChoiceStarted(DialogueChoicesNode choicesNode)
    {
        SortChoices(choicesNode);
        _buttonContainer.gameObject.SetActive(true);
        NukeButtons();
        CreateButtons();
    }

    private void SortChoices(DialogueChoicesNode choicesNode)
    {
        _sortedChoices.Clear();
        _sortedChoices.AddRange(choicesNode.DialogueChoices);
        
        _sortedChoices.Sort((choice1, choice2) =>
        {
            if (choice1.Taken != choice2.Taken)
            {
                if (choice1.Taken)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            return 0;
        });
    }

    void CreateButtons()
    {
        for (int i = 0; i < _sortedChoices.Count; i++)
        {
            var choiceButton = Instantiate(_buttonPrefab, _buttonContainer);
            choiceButton.SetChoiceData(_sortedChoices[i]);
            choiceButton.ChoiceSelected += HandleDialogueChoiceSelected;
            _spawnedButtons.Add(choiceButton);
        }
    }

    public void HandleDialogueChoiceSelected(int index)
    {
        DialogueSystem.Instance.PickChoice(index);
    }

    private void NukeButtons()
    {
        for (int i = 0; i < _spawnedButtons.Count; i++)
        {
            Debug.Log(_spawnedButtons[i].gameObject);
            Destroy(_spawnedButtons[i].gameObject);
        }
        _spawnedButtons.Clear();
    }
}
