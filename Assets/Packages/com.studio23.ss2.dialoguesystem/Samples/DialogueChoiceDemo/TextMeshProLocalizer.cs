using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using Object = System.Object;


/// <summary>
/// use for variable text that has a perpetually changing stringreference
/// EX: UI View for item descriptions.
/// This'll let us load a text immediately and let the localized text load in the background
/// </summary>
public class TextMeshProLocalizer : MonoBehaviour
{
    public TMP_Text TmpTarget => tmpTarget;
    [SerializeField] private TMP_Text tmpTarget;
    [SerializeField] private LocalizeStringEvent localizer;
    public LocalizeStringEvent Localizer => localizer;
    public string Text => tmpTarget.text;

    private void Awake()
    {
        init();
    }

    [ContextMenu("INIT")]
    void init()
    {
        if(tmpTarget == null)
            tmpTarget = GetComponent<TextMeshPro>();
        if (tmpTarget == null)
            tmpTarget = GetComponent<TextMeshProUGUI>();
        if (localizer == null)
            localizer = GetComponent<LocalizeStringEvent>();
    }
    
    /// <summary>
    /// TMP immediately shows tempText.
    /// Localization load done => show localized string
    /// </summary>
    /// <param name="tempText"></param>
    /// <param name="localizedString"></param>
    public void SetText(LocalizedString localizedString, string tempText= "")
    {
        tmpTarget.text = tempText;
        localizer.StringReference = localizedString;
        localizer.RefreshString();
    }
    
    /// <summary>
    /// TMP immediately shows tempText.
    /// Localization load done => show localized string
    /// </summary>
    /// <param name="tempText"></param>
    /// <param name="localizedString"></param>
    public async UniTask SetTextAndWait(LocalizedString localizedString, string tempText= "")
    {
        Debug.Log("wait start " + Time.time);
        SetText(localizedString, tempText);

        tmpTarget.text = await localizer.StringReference.GetLocalizedStringAsync();
    }
    
    /// <summary>
    /// TMP immediately shows tempText.
    /// Localization load done => show localized string
    /// </summary>
    /// <param name="tempText"></param>
    /// <param name="localizedString"></param>
    public async UniTask<string> LoadTextAndWait(LocalizedString localizedString, string tempText= "")
    {
        SetText(localizedString, tempText);
        return await localizer.StringReference.GetLocalizedStringAsync();
    }

    [ContextMenu("adsda")]
    public void foo()
    {
        Debug.Log(tmpTarget.text, tmpTarget);
    }

    /// <summary>
    /// Pass an args array of objects. 
    /// YOU MUST SET THE SMART FIELD FOR ALL LANGUAGES TO SMART IN EDITOR FOR THE COMPONENT
    /// smart strings format would be $"abcd{0}efg{1}..."
    /// Sufficient for this game.
    /// </summary>
    /// <param name="tempText"></param>
    /// <param name="localizedString"></param>
    /// <param name="args"></param>
    public void SetText(string tempText, LocalizedString localizedString, Object[] args)
    {
        tmpTarget.text = tempText;
        localizer.StringReference = localizedString;
        setArgsAndRefresh(args);
    }
    
    public void SetText(string tempText, Object[] args)
    {
        tmpTarget.text = tempText;
        setArgsAndRefresh(args);
    }

    public void setArgsAndRefresh(Object[] args)
    {
        localizer.StringReference.Arguments = args;
        localizer.RefreshString();
    }
    
    public void setArgsAndRefresh(List<Object> args)
    {
        localizer.StringReference.Arguments = args;
        localizer.RefreshString();
    }

    public void SetTextNoLocalization(string s)
    {
        tmpTarget.text = s;
    }
}
