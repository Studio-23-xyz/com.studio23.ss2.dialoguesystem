using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Studio23.SS2.DialogueSystem.Utility;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    public abstract class DialogueLineNodeBase : DialogueNodeBase
    {
        public LocalizedString DialogueLocalizedString;
        [Header("Character Data")]
        public string ID;
        public string Reaction;

        [Header("Sound")]
        public string FMODEvent;
        
        private bool _canAdvanceDialogue;
        
        [Node.Output(typeConstraint = TypeConstraint.Strict, connectionType = ConnectionType.Override)]
        public DialogueLineNodeBase Exit;
        
        [Output(typeConstraint =  TypeConstraint.InheritedInverse)] 
        public EventNodeBase Events;

        public override void HandleDialogueAdvance()
        {
            _canAdvanceDialogue = true;
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
            _canAdvanceDialogue = false;

            Core.DialogueSystem.Instance.DialogueLineStarted?.Invoke(this);
            while (!_canAdvanceDialogue)
            {
                await UniTask.Yield();
            }
            Core.DialogueSystem.Instance.DialogueLineCompleted?.Invoke(this);
            
            InvokePostPlayEvents();
        }

        public override DialogueNodeBase GetNextNode()
        {
            NodePort outputPort = GetExitPort();
            if (outputPort == null || outputPort.Connection == null)
            {
                return null;
            }
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

        public override void Initialize()
        {
            
        }
    }

}

