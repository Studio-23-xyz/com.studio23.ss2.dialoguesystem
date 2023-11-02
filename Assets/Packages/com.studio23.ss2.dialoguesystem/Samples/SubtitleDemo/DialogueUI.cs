using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Core;
using Studio23.SS2.DialogueSystem.Data;
using System;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace Studio23.SS2.DialogueSystem.Samples
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("UI Components")]
        public TextMeshProUGUI DialogueText;
        public Image BackgroundImage;
        public GameObject UIRoot;

        public CharacterTable CharacterTable;

        [Header("Configuration")]
        [SerializeField] private SubtitileSettings _config;




        void Start()
        {
            CharacterTable = Resources.Load<CharacterTable>("DialogueSystem/CharacterTable");
            ApplyConfiguration();
            RegisterEvents();
        }

        private void ApplyConfiguration()
        {
            DialogueText.fontSize = _config.subtitleFontSize;
            DialogueText.color = _config.subtitleColor;
            BackgroundImage.gameObject.SetActive(_config.ShowBackground);
            BackgroundImage.color = _config.BackGroundColor;
        }


        private void RegisterEvents()
        {
            DialogueManager.Instance.OnDialogueStart += async ()=> await PlayDialogueAsync(); 
        }


        private async Task PlayDialogueAsync()
        {
            ShowUI(true);
            DialogueBase node = DialogueManager.Instance.GetNextNode();

            while (node!=null)
            {
                await ShowDialogueTextAsync(node);//TODO Implement a audio playing mechanic here
                node = DialogueManager.Instance.GetNextNode();
            }

            ShowUI(false);

        }

        private void ShowUI(bool state)
        {
            UIRoot.SetActive(state);
        }

        private async Task ShowDialogueTextAsync(DialogueBase node)
        {
            CharacterData characterData = CharacterTable.GetCharacterData(node.CharacterID);
            string text = string.Empty;
            if (_config.EnableCharacterColor)
            {
               text = $"<color=#{characterData.DialogueColor.ToHexString()}>{characterData.CharacterName}</color>:{node.DialogueText}";
            }
            else
            {
                text = $"{characterData.CharacterName}:{node.DialogueText}";
            }
           
            DialogueText.text = text;
            await UniTask.Delay(TimeSpan.FromSeconds(5), ignoreTimeScale: false);//TODO Dynamic Wait time according to text length
           
        }

    }
}