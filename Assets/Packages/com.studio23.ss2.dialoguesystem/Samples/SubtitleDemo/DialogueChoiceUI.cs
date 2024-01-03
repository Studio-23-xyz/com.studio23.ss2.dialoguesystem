using System;
using System.Collections.Generic;
using Studio23.SS2.DialogueSystem.Core;
using Studio23.SS2.DialogueSystem.Data;
using UnityEngine;

namespace Packages.com.studio23.ss2.dialoguesystem.Samples.SubtitleDemo
{
    public class DialogueChoiceUI:MonoBehaviour
    {
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
            _buttonContainer.gameObject.SetActive(true);
            NukeButtons();
            CreateButtons(choicesNode);
        }

        void CreateButtons(DialogueChoicesNode choicesNode)
        {
            for (int i = 0; i < choicesNode.DialogueChoices.Count; i++)
            {
                var choiceButton = Instantiate(_buttonPrefab, _buttonContainer);
                choiceButton.SetChoiceData(i, choicesNode.DialogueChoices[i]);
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
}