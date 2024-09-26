using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Studio23.SS2.DialogueSystem.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;

namespace Studio23.SS2.DialogueSystem.Data
{
    /// <summary>
    /// Parent branching Node that has conditional children
    /// Because order is unreliable 
    /// </summary>
    [NodeTint("#0000AA"), CreateNodeMenu("Dialogue Conditional Branch")]
    public class DialogueConditionalBranchNode : DialogueBranchingNode
    {
        [Input]
        public DialogueLineNodeBase Entry;
        [Output]
        public DialogueConditionalNodeBase ConditionalBranches;
        /// #TODO use list output instead of simple multi output attribute
        List<DialogueConditionalNodeBase> _conditionalBranchesCache;
        [Output(connectionType = ConnectionType.Override)] 
        public DialogueLineNodeBase DefaultBranch;

        [CanBeNull] public DialogueLineNodeBase GetDefaultBranchNode()
        {
            NodePort outputPort = GetOutputPort("DefaultBranch");
            if (outputPort == null || outputPort.Connection == null)
            {
                return null;
            }
            return outputPort.Connection.node as DialogueLineNodeBase;
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == "ConditionalBranches")
            {
                return this;
            }
            if (port.fieldName == "DefaultBranch")
            {
                return this;
            }
            return base.GetValue(port);
        }

        public List<DialogueConditionalNodeBase> FetchAllConditionalBranchNodes()
        {
            if (_conditionalBranchesCache == null)
            {
                _conditionalBranchesCache = new ();
            }
            else
            {
                _conditionalBranchesCache.Clear();
            }

            this.GetOutputNodesConnectedToPort("ConditionalBranches", _conditionalBranchesCache);
            return _conditionalBranchesCache;
        }

        public override void HandleDialogueAdvance()
        {
            //do nothing
        }

        public override DialogueNodeBase GetNextNode()
        {
            foreach (var conditionalBranchNode in _conditionalBranchesCache)
            {
                var result = conditionalBranchNode.CheckConditions();
                Debug.Log($"{conditionalBranchNode} evalute {result}", conditionalBranchNode);
                if (result)
                {
                    return conditionalBranchNode;
                }
            }

            var defaultBranchNode = GetDefaultBranchNode();
            Debug.Log($"none of {_conditionalBranchesCache.Count} branches. RETURN default NODE {(defaultBranchNode == null ? "null" : defaultBranchNode)}", defaultBranchNode);
            return defaultBranchNode;
        }

        /// <inheritdoc />
        public override IEnumerable<DialogueNodeBase> GetConnectedNodes()
        {
            //#TODO can be optimized
            //not priority since only exporter uses this func
            foreach (var choiceNode in FetchAllConditionalBranchNodes())
            {
                yield return choiceNode;
            }
            var forceExitNode = GetDefaultBranchNode();
            if (forceExitNode != null)
            {
                yield return forceExitNode;
            }
        }

        public override void HandleChoiceSelected(int choiceIndex)
        {
            //Do nothing
        }

        public override UniTask Play()
        {
            FetchAllConditionalBranchNodes();
            return UniTask.CompletedTask;
        }

        public override void Initialize()
        {
            //do nothing
        }
    }
}
