
using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Core;
using Studio23.SS2.DialogueSystem.Data;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Studio23.SS2.DialogueSystem.UI
{

    public class DialogueUI : MonoBehaviour
    {
        [Header("UI Components")]
        public TextMeshProUGUI dialogueText;
        public Image BackgroundImage;
        public GameObject UIRoot;
        public Button NextButton;
        public Button SkipButton;

        private DialogueGraph _currentGraph;
        [SerializeField]private DialogueState dialogueState;

        [Header("Configuration")]
        [SerializeField]private DialogueUIConfig config;



      

        private CancellationTokenSource CancelCurrentDialogueLineToken;


        void Start()
        {
            NextButton.onClick.AddListener(Next);
            ApplyConfiguration();
        }

        private void ApplyConfiguration()
        {
            dialogueText.fontSize = config.subtitleFontSize;
            dialogueText.color = config.subtitleColor;
            BackgroundImage.gameObject.SetActive(config.ShowBackground);
            BackgroundImage.color = config.BackGroundColor;
        }


        public void RegisterGraph(DialogueGraph newGraph)
        {
            _currentGraph = newGraph;
            _currentGraph.ClearEvents();
            _currentGraph.OnDialogueStart += () => PlayDialogue();
            _currentGraph.OnDialogueComplete += () => EndDialogue();
            _currentGraph.OnDialogueNext += async () => await ShowDialogueText(_currentGraph.CurrentNode.DialogueText);
        }


        private async void PlayDialogue()
        {
            if (dialogueState != DialogueState.Ended)
            {
                EndDialogue();
            }
            await ShowDialogueText(_currentGraph.CurrentNode.DialogueText);
        }

        private void ShowUI(bool state)
        {
            UIRoot.SetActive(state);
            
        }

        private async UniTask ShowDialogueText(string text)
        {

            dialogueText.text = string.Empty;
            dialogueState = DialogueState.Started;
            CancelCurrentDialogueLineToken = new CancellationTokenSource();

            ShowUI(true);
            for (int i = 0; i < text.Length; i++)
            {
                await UniTask.Delay((int)(config.letterDelay * 1000), cancellationToken: CancelCurrentDialogueLineToken.Token).SuppressCancellationThrow();
                if (dialogueState == DialogueState.Skipped) break;
                dialogueText.text += text[i];

            }

            await UniTask.Delay((int)(text.Length * 100 * config.nextSentenceDelayMultiplier), cancellationToken: CancelCurrentDialogueLineToken.Token).SuppressCancellationThrow();
            dialogueText.text = string.Empty;
            _currentGraph.NextNode();

        }

        private void Next()
        {
            dialogueState = DialogueState.Skipped;
            CancelCurrentDialogueLineToken.Cancel();
        }

        private void EndDialogue()
        {
            dialogueState = DialogueState.Ended;
            ShowUI(false);
           
        }
    }
}