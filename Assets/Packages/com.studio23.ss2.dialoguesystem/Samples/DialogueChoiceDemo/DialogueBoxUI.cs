using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Core;
using Studio23.SS2.DialogueSystem.Data;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;
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

    void Start()
    {
        ApplyConfiguration();
        HideUI();
        RegisterEvents();
    }


    private void OnDestroy()
    {
        UnRegisterEvents();
    }

    private void ApplyConfiguration()
    {
        DialogueTMP.fontSize = _config.SubtitleFontSize;
        DialogueTMP.color = _config.SubtitleColor;
        BackgroundImage.gameObject.SetActive(_config.ShowBackground);
        BackgroundImage.color = _config.BackGroundColor;
    }
    

    private void RegisterEvents()
    {
        OnDialogueAdvanced += DialogueSystem.Instance.AdvanceDialogue;
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
            OnDialogueAdvanced -= DialogueSystem.Instance.AdvanceDialogue;
            DialogueSystem.Instance.OnDialogueStarted -=  ShowUI;
            DialogueSystem.Instance.OnDialogueEnded -=  HideUI;
            DialogueSystem.Instance.DialogueLineStarted -= handleDialogueLineStarted;

            DialogueSystem.Instance.OnDialogueChoiceStarted -= HideUI;
            DialogueSystem.Instance.OnDialogueChoiceEnded -= ShowUI;
        }
    }

 
    public void handleDialogueLineStarted(DialogueLineNodeBase dialogueLineNodeBase)
    {
        ShowDialogueTextAsync(dialogueLineNodeBase);
    }
    public async UniTask ShowDialogueTextAsync(DialogueLineNodeBase nodeBase)
    {
        // DialogueTMP.text =  await TextLocalizer.LoadTextAndWait(nodeBase.DialogueLocalizedString);
        DialogueTMP.text =  await nodeBase.DialogueLocalizedString.GetLocalizedStringAsync();
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

    public void ShowUI()
    {
        UIRoot.SetActive(true);
        BackgroundImage.gameObject.SetActive(true);
    }

    public void HideUI(DialogueGraph graph)
    {
        HideUI();
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