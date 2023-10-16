
using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Studio23.SS2.DialogueSystem.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("UI Components")]
        public TextMeshProUGUI DialogueText;
        public Image BackgroundImage;
        public GameObject UIRoot;
        public Button FastForwardButton;
        public Button SkipButton;

        private DialogueGraph _currentGraph;
        [SerializeField] private DialogueState _dialogueState;

        [Header("Configuration")]
        [SerializeField] private DialogueUIConfig _config;

        void Start()
        {
            FastForwardButton.onClick.AddListener(FastForward);
            ApplyConfiguration();
        }

        private void ApplyConfiguration()
        {
            DialogueText.fontSize = _config.subtitleFontSize;
            DialogueText.color = _config.subtitleColor;
            BackgroundImage.gameObject.SetActive(_config.ShowBackground);
            BackgroundImage.color = _config.BackGroundColor;
        }


        public void RegisterGraph(DialogueGraph newGraph)
        {
            _currentGraph = newGraph;
            _currentGraph.ClearEvents();
            _currentGraph.OnDialogueStart += PlayDialogue;

            _currentGraph.OnDialogueNext += async (node) => await ShowDialogueTextAnimated(node.DialogueText);


            _currentGraph.OnDialogueComplete += () => EndDialogue();
        }


        private async void PlayDialogue(DialogueBase node)
        {
            ShowUI(true);
            await ShowDialogueTextAnimated(node.DialogueText);
        }

        private void ShowUI(bool state)
        {
            UIRoot.SetActive(state);
        }

        private async UniTask ShowDialogueTextAnimated(string text)
        {

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
            _currentGraph.NextNode();

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