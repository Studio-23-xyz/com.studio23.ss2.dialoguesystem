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

    public static DialogueBoxUI Instance;

    public event Action OnDialogueAdvanced;

    private void Awake()
    {
        Instance = this;
        director = GameObject.FindObjectOfType<PlayableDirector>();
    }

    void Start()
    {
        RegisterEvents();
    }
    public PlayableDirector director;

    public void testTimelineResume()
    {
        // director.Pause();
        director.time = EndTime;
        rootPlayable.SetSpeed(1);
        Debug.Log($"resume director.time {director.time}: -> {EndTime}");

    }

    private Playable rootPlayable;
    [SerializeField] private double EndTime;

    public void Pause(Playable playable, double endTime)
    {
        rootPlayable = playable;
        EndTime = endTime;
        Debug.Log($"pause director.time {director.time}: -> {EndTime}");
        rootPlayable.SetSpeed(0);
    }

    private void OnDestroy()
    {
        UnRegisterEvents();
    }


    private void RegisterEvents()
    {
        OnDialogueAdvanced += DialogueSystem.Instance.AdvanceDialogue;
        DialogueSystem.Instance.OnDialogueStarted +=  ShowUI;
        DialogueSystem.Instance.OnDialogueEnded +=  HandleDialogueEnded;
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
            DialogueSystem.Instance.OnDialogueEnded -=  HandleDialogueEnded;
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
        Debug.Log($"ShowUI {BackgroundImage.gameObject.activeSelf}");

    }

    public void HandleDialogueEnded(DialogueGraph graph)
    {
        testTimelineResume();
        HideUI();
    }

    public void HideUI()
    {
        Debug.Log("hideUI");
        UIRoot.SetActive(false);
        BackgroundImage.gameObject.SetActive(false);
    }

   

    public void handleDialogueAdvance()
    {
        OnDialogueAdvanced?.Invoke();
    }
}