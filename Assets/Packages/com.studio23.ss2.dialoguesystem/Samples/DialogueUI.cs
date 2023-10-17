using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Core;
using Studio23.SS2.DialogueSystem.Data;
using TMPro;
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
        public Button FastForwardButton;
        public Button SkipButton;


        public DialogueEvents.DialogueEvent OnDialoguePlayComplete;

        [SerializeField] private DialogueState _dialogueState;

        [Header("Configuration")]
        [SerializeField] private DialogueUIConfig _config;




        void Start()
        {
            FastForwardButton.onClick.AddListener(FastForward);
            ApplyConfiguration();
            RegisterGraph();
        }

        private void ApplyConfiguration()
        {
            DialogueText.fontSize = _config.subtitleFontSize;
            DialogueText.color = _config.subtitleColor;
            BackgroundImage.gameObject.SetActive(_config.ShowBackground);
            BackgroundImage.color = _config.BackGroundColor;
        }


        private void RegisterGraph()
        {

            DialogueManager.Instance.OnDialogueStart += PlayDialogue;
            DialogueManager.Instance.OnDialogueNext += async (node)=> await ShowDialogueTextAnimated(node);
            DialogueManager.Instance.OnDialogueComplete += () => EndDialogue();
            OnDialoguePlayComplete += DialogueManager.Instance.GetNextNode;
        }


        private async void PlayDialogue(DialogueBase node)
        {
            ShowUI(true);
            await ShowDialogueTextAnimated(node);
        }

        private void ShowUI(bool state)
        {
            UIRoot.SetActive(state);
        }

        private async UniTask ShowDialogueTextAnimated(DialogueBase node)
        {

            string text = $"{node.CharacterInfo.CharacterName}:{node.DialogueText}";

            DialogueText.text = string.Empty;
            _dialogueState = DialogueState.Started;



            for (int i = 0; i < text.Length; i++)
            {
                DialogueText.text += text[i];
                await UniTask.Delay((int)(_config.letterDelay * 1000));
                if (_dialogueState == DialogueState.FastForwarded)
                {
                    DialogueText.text = text;
                    break;
                }

            }
            await UniTask.Delay((int)(text.Length * 100 * _config.nextSentenceDelayMultiplier));
            OnDialoguePlayComplete?.Invoke();

        }



        private void FastForward()
        {
            _dialogueState = DialogueState.FastForwarded;
        }

        private void EndDialogue()
        {
            _dialogueState = DialogueState.Ended;
            ShowUI(false);

        }
    }
}