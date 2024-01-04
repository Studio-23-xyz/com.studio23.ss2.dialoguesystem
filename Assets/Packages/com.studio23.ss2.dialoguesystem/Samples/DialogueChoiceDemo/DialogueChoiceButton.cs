using System;
using Studio23.SS2.DialogueSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoiceButton:MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProLocalizer _textLocalizer;
    [SerializeField] private int _choiceIndex = -1;
    public event Action<int> ChoiceSelected;


    private void Awake()
    {
        if (_button == null)
        {
            _button = GetComponent<Button>();
        }
        
        _button.onClick.AddListener(()=> ChoiceSelected?.Invoke(_choiceIndex));
    }

    public void SetChoiceData(DialogueChoiceNodeBase choice)
    {
        _image.color = choice.Taken ? Color.gray : Color.green;
        //#TODO this doesn't wait for the text to be loaded. Just sets and starts loading
        // we should wait for all the option text to be loaded
        // or preload all option text
        // currently enough for demo I guess
        _textLocalizer.SetText(choice.DialogueLocalizedString);
        _choiceIndex = choice.DialogueChoiceIndex;
    }
    
}
