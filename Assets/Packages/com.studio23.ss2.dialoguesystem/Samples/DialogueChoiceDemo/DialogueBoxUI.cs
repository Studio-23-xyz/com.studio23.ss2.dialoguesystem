using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Core;
using Studio23.SS2.DialogueSystem.Data;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxUI : MonoBehaviour
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
        //#TODO THE UI Shouldn't be responsible for loading the character table
        CharacterTable = Resources.Load<CharacterTable>("DialogueSystem/CharacterTable");
        ApplyConfiguration();
        RegisterEvents();
    }


    private void OnDestroy()
    {
        UnRegisterEvents();
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
        DialogueSystem.Instance.OnDialogueStarted +=  ShowUI;
        DialogueSystem.Instance.OnDialogueEnded +=  HideUI;
        DialogueSystem.Instance.DialogueLineStarted += handleDialogueLineStarted;

        DialogueSystem.Instance.OnDialogueChoiceStarted += HideUI;
        DialogueSystem.Instance.OnDialogueChoiceEnded += ShowUI;
    }
    
    private void UnRegisterEvents()
    {
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.OnDialogueStarted -=  ShowUI;
            DialogueSystem.Instance.OnDialogueEnded -=  HideUI;
            DialogueSystem.Instance.DialogueLineStarted -= handleDialogueLineStarted;

            DialogueSystem.Instance.OnDialogueChoiceStarted -= HideUI;
            DialogueSystem.Instance.OnDialogueChoiceEnded -= ShowUI;
        }
    }

 
    private void handleDialogueLineStarted(DialogueLineNodeBase dialogueLineNodeBase)
    {
        ShowDialogueTextAsync(dialogueLineNodeBase);
    }
    private async UniTask ShowDialogueTextAsync(DialogueLineNodeBase nodeBase)
    {
        string text = string.Empty;

        CharacterData characterData = CharacterTable.GetCharacterData(nodeBase.ID);
        if (characterData != null)
        {
            if (_config.EnableCharacterColor)
            {
                text = $"<color=#{characterData.DialogueColor.ToHexString()}>{characterData.CharacterName}</color>:{nodeBase.DialogueText}";
            }
            else
            {
                text = $"{characterData.CharacterName}:{nodeBase.DialogueText}";
            }
        }
        else
        {
            text = nodeBase.DialogueText;
        }

        DialogueText.text = text;
        await UniTask.Delay(TimeSpan.FromSeconds(5), ignoreTimeScale: false);//TODO Dynamic Wait time according to text length
    }

    private void HideUI(DialogueChoicesNode obj)
    {
        HideUI();
    }
    
    private void ShowUI(DialogueChoicesNode obj)
    {
        ShowUI();
    }

    private void ShowUI(DialogueGraph graph)
    {
        ShowUI();
    }

    private void ShowUI()
    {
        UIRoot.SetActive(true);
    }

    private void HideUI(DialogueGraph graph)
    {
        HideUI();
    }

    private void HideUI()
    {
        UIRoot.SetActive(false);
    }

   

    public void handleDialogueAdvance()
    {
        DialogueSystem.Instance.AdvanceDialogue();
    }
}