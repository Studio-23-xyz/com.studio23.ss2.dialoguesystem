using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Core;
using Studio23.SS2.DialogueSystem.Data;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogueBoxUI : MonoBehaviour
{
    [FormerlySerializedAs("DialogueText")] 
    [Header("UI Components")]
    public TextMeshProUGUI DialogueTMP;
    public TextMeshProLocalizer TextLocalizer;
    public Image BackgroundImage;
    public GameObject UIRoot;

    [Header("Configuration")]
    [SerializeField] private SubtitleSettings _config;

    public event Action OnDialogueAdvanced;

    public void ShowDialogueLine(DialogueLineNodeBase dialogueLineNodeBase)
    {
        ShowDialogueTextAsync(dialogueLineNodeBase).Forget();
    }
    public async UniTask ShowDialogueTextAsync(DialogueLineNodeBase nodeBase)
    {
        DialogueTMP.text =  await nodeBase.DialogueLocalizedString.GetLocalizedStringAsync();
    }

    public void ShowDialogueLineImmediate(DialogueLineNodeBase nodeBase)
    {
        DialogueTMP.text =  nodeBase.DialogueLocalizedString.GetLocalizedString();
    }

    public void ShowUI()
    {
        UIRoot.SetActive(true);
        BackgroundImage.gameObject.SetActive(true);
    }


    public void HideUI()
    {
        UIRoot.SetActive(false);
        BackgroundImage.gameObject.SetActive(false);
    }

   

    public void handleDialogueAdvance()
    {
        OnDialogueAdvanced?.Invoke();
    }
}