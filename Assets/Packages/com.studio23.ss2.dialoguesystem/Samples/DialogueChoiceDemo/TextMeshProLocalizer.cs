using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using Object = System.Object;


/// <summary>
/// use for variable text that has a perpetually changing stringreference
/// EX: UI View for item descriptions.
/// This'll let us load a text immediately and let the localized text load in the background
/// </summary>
public class TextMeshProLocalizer : MonoBehaviour
{
    public TMP_Text TmpTarget => _tmpTarget;
    [SerializeField] private TMP_Text _tmpTarget;
    [SerializeField] private LocalizeStringEvent _localizer;
    public LocalizeStringEvent Localizer => _localizer;
    public string Text => _tmpTarget.text;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        //made to work with both UI and non UI textmeshpro
        if(_tmpTarget == null)
            _tmpTarget = GetComponent<TextMeshPro>();
        if (_tmpTarget == null)
            _tmpTarget = GetComponent<TextMeshProUGUI>();
        if (_localizer == null)
            _localizer = GetComponent<LocalizeStringEvent>();
    }
    
    /// <summary>
    /// TMP immediately shows tempText.
    /// Localization load done => show localized string
    /// </summary>
    /// <param name="tempText"></param>
    /// <param name="localizedString"></param>
    public void SetText(LocalizedString localizedString, string tempText= "")
    {
        _tmpTarget.text = tempText;
        _localizer.StringReference = localizedString;
        _localizer.RefreshString();
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

        _tmpTarget.text = await _localizer.StringReference.GetLocalizedStringAsync();
    }
    
    /// <summary>
    /// TMP immediately shows tempText.
    /// Localization load done return actual string in correct language
    /// </summary>
    /// <param name="tempText"></param>
    /// <param name="localizedString"></param>
    public async UniTask<string> LoadTextAndWait(LocalizedString localizedString, string tempText= "")
    {
        SetText(localizedString, tempText);
        return await _localizer.StringReference.GetLocalizedStringAsync();
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
        _tmpTarget.text = tempText;
        _localizer.StringReference = localizedString;
        SetArgsAndRefresh(args);
    }
    
    public void SetText(string tempText, Object[] args)
    {
        _tmpTarget.text = tempText;
        SetArgsAndRefresh(args);
    }
    
    public void SetText(string tempText)
    {
        _tmpTarget.text = tempText;
    }
    

    public void SetArgsAndRefresh(Object[] args)
    {
        _localizer.StringReference.Arguments = args;
        _localizer.RefreshString();
    }
    
    public void SetArgsAndRefresh(List<Object> args)
    {
        _localizer.StringReference.Arguments = args;
        _localizer.RefreshString();
    }

    public void SetTextNoLocalization(string s)
    {
        _tmpTarget.text = s;
    }
}
