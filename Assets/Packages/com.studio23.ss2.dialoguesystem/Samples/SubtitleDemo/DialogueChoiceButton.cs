using System;
using Studio23.SS2.DialogueSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace Packages.com.studio23.ss2.dialoguesystem.Samples.SubtitleDemo
{
    public class DialogueChoiceButton:MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
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

        public void SetChoiceData(int choiceIndex, DialogueChoiceNodeBase choice)
        {
            _image.color = choice.Taken ? Color.gray : Color.green;
            _text.text = choice.DialogueText;
            _choiceIndex = choiceIndex;
        }
        
    }
}