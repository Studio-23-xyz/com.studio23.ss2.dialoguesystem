using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Core;
using Studio23.SS2.DialogueSystem.Data;
using System;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class DialogueUI : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI DialogueText;
    public Image BackgroundImage;
    public GameObject UIRoot;

    public CharacterTable CharacterTable;

    [Header("Configuration")]
    [SerializeField] private SubtitleSettings _config;


    void Start()
    {
        CharacterTable = Resources.Load<CharacterTable>("DialogueSystem/CharacterTable");
        ApplyConfiguration();
        RegisterEvents();
    }

    private void ApplyConfiguration()
    {
        DialogueText.fontSize = _config.SubtitleFontSize;
        DialogueText.color = _config.SubtitleColor;
        BackgroundImage.gameObject.SetActive(_config.ShowBackground);
        BackgroundImage.color = _config.BackGroundColor;
    }
    

    private void RegisterEvents()
    {
        DialogueSystem.Instance.OnDialogueStarted += (graph) => ShowUI(true);
        DialogueSystem.Instance.OnDialogueEnded +=  (graph) => ShowUI(false);

        DialogueSystem.Instance.DialogueLineStarted += handleDialogueLineStarted;
    }

    private void handleDialogueLineStarted(DialogueLineNode dialogueLineNode)
    {
        ShowDialogueTextAsync(dialogueLineNode);
    }

    private void ShowUI(bool state)
    {
        UIRoot.SetActive(state);
    }

    private async UniTask ShowDialogueTextAsync(DialogueLineNode node)
    {
        
        string text = string.Empty;

        CharacterData characterData = CharacterTable.GetCharacterData(node.ID);
        if (characterData != null)
        {
            if (_config.EnableCharacterColor)
            {
                text = $"<color=#{characterData.DialogueColor.ToHexString()}>{characterData.CharacterName}</color>:{node.DialogueText}";
            }
            else
            {
                text = $"{characterData.CharacterName}:{node.DialogueText}";
            }
        }
        else
        {
            text = node.DialogueText;
        }

        DialogueText.text = text;
        await UniTask.Delay(TimeSpan.FromSeconds(5), ignoreTimeScale: false);//TODO Dynamic Wait time according to text length

    }


    public void handleDialogueAdvance()
    {
        DialogueSystem.Instance.AdvanceDialogue();
    }
}