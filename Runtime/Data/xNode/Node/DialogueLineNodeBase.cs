using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Utility;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Serialization;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    public abstract class DialogueLineNodeBase : DialogueNodeBase
    {
        public LocalizedString DialogueLocalizedString;
        [Header("Character Data")] 
        public LineSpeakerData SpeakerData;

        private bool _hasStarted = false;
        private bool _hasCompleted = false;

        public bool HasStarted=> _hasStarted;
        public bool HasCompleted => _hasCompleted;
        
        private bool _canAdvanceDialogue;
        
        [Node.Output(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)]
        public DialogueLineNodeBase Exit;
        
        [Output(typeConstraint =  TypeConstraint.InheritedInverse)] 
        public EventNodeBase Events;

        private CancellationTokenSource _cancelDialogueLine;

        public override void HandleDialogueAdvance()
        {
            _canAdvanceDialogue = true;
        }

        public override void HandleDialogueCancel()
        {
            if(_cancelDialogueLine != null)
                _cancelDialogueLine.Cancel();
        }
        
        public virtual void InvokePostPlayEvents()
        {
            var eventNodePort= GetOutputPort("Events");
            for (int i = 0; i < eventNodePort.ConnectionCount; i++)
            {
                var eventNodeConnection = eventNodePort.GetConnection(i);
                if (eventNodeConnection != null)
                {
                    if (eventNodeConnection.node is EventNodeBase eventNode)
                    {
                        eventNode.Invoke();
                    }
                    else
                    {
                        Debug.LogWarning($"{this} Events port no {i} is connected to {eventNodeConnection.node} but it's not an event node", this);
                    }
                }
                else
                {
                    Debug.LogWarning($"{this} Events port no {i} is connected to {eventNodeConnection} but connection null", this);
                }
            }
        }


        public override void HandleChoiceSelected(int choiceIndex)
        {
            //do nothing
        }

        public override async UniTask Play()
        {
            HandleDialogueCancel();
            _cancelDialogueLine = new CancellationTokenSource();
            _canAdvanceDialogue = false;
            HandleDialogueCompleted(true);
            if (!Core.DialogueSystem.Instance.IsSkipActive || 
                Core.DialogueSystem.Instance.ShouldShowLineWhenSkipped)
            {
                Core.DialogueSystem.Instance.DialogueLineStarted?.Invoke(this);
                while (!_canAdvanceDialogue && !_cancelDialogueLine.IsCancellationRequested)
                {
                    //at this point, we are showing dialogue with ShouldShowLineWhenSkipped = true
                    //if (Core.DialogueSystem.Instance.IsSkipActive)
                    //{
                    //    await UniTask.Delay(TimeSpan.FromSeconds(Core.DialogueSystem.Instance.ShowLineDurationWhenSkipping), cancellationToken: _cancelDialogueLine.Token).SuppressCancellationThrow();
                    //    break;
                    //}

                    if(await UniTask.Yield(cancellationToken: _cancelDialogueLine.Token).SuppressCancellationThrow())
                    {
                        Debug.Log($"CT10 Dialogue Line Cancelled {graph.name}");
                        break;
                    }
                    if(await UniTask.NextFrame(cancellationToken: _cancelDialogueLine.Token).SuppressCancellationThrow())
                    {
                        Debug.Log($"CT10 Dialogue Line Cancelled {graph.name}");
                        break;
                    }
                }
                Core.DialogueSystem.Instance.DialogueLineCompleted?.Invoke(this);
            }
            
            InvokePostPlayEvents();
        }

        public override DialogueNodeBase GetNextNode()
        {
            NodePort outputPort = GetExitPort();
            if (outputPort == null || outputPort.Connection == null)
            {
                return null;
            }
            if (outputPort.Connection.node is DialogueLineNodeBase nextDialogueLine)
                nextDialogueLine.HandleDialogueStarted(true);
            return outputPort.Connection.node as DialogueNodeBase;
        }
        
        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "Exit")
            {
                return this;
            }else if (port.fieldName == "Events")
            {
                return null;
            }
            return base.GetValue(port);
        }

        public void SetLocalizationTable(TableReference table)
        {
            DialogueLocalizedString = new LocalizedString();
            DialogueLocalizedString.TableReference = table;
        }
        
        public TableReference GetLocalizationTable()
        {
            if (DialogueLocalizedString == null)
            {
                return default;
            }
            return DialogueLocalizedString.TableReference;
        }

        public string GetLocalizedLineTextInstant() => DialogueLocalizedString.GetLocalizedString();
        public override void Initialize()
        {
            HandleDialogueCompleted(false);
            HandleDialogueStarted(false);
        }

        public void HandleDialogueCompleted(bool hasCompleted)
        {
            _hasCompleted = hasCompleted;
        }

        public void HandleDialogueStarted(bool hasStarted)
        {
            _hasStarted = hasStarted;
        }
    }

}

