using System.Collections.Generic;
using Studio23.SS2.DialogueSystem.Data;
using UnityEngine;
using XNode;

namespace  Studio23.SS2.DialogueSystem.Utility
{
    public static class XNodeExtensions
    {
        /// <summary>
        /// Given an array node, Get connected nodes
        /// </summary>
        /// <param name="node"></param>
        /// <param name="fieldName">DON'T PASS AN ARRAY FIELD DAMMIT</param>
        /// <param name="count"></param>
        /// <param name="outputList"></param>
        /// <typeparam name="T"></typeparam>
        public static void GetOutputNodesConnectedToPort<T>(this Node node, string fieldName, List<T> outputList)
        {
            //if the port is not an array, we can actually use connections.count
            var port = node.GetOutputPort(fieldName);
            for (int i = 0; i < port.ConnectionCount; i++)
            {
                var connection = port.GetConnection(i);
                if (connection != null)
                {
                    if (connection.node is T tNode)
                    {
                        outputList.Add(tNode);
                    }
                    else
                    {
                        # if UNITY_EDITOR
                            Debug.LogWarning($"{node} Events port no {i} is connected to {connection.node} but it's not a {typeof(T)}");
                        # endif
                    }
                }
            }
            
        }
        
        /// <summary>
        /// Given an array node, Get connected nodes
        /// NOTE: ENSURE THAT THE FIELD IS AN ARRAY.
        /// OR BE A SANE PERSON AND USE A NON ARRAY FIELD + getOutputNodesConnectedToPort()
        /// INSTEAD OF THIS MONSTROSITY
        /// </summary>
        /// <param name="node"></param>
        /// <param name="fieldName">FIELD MUST HAVE AN ARRAY TYPE</param>
        /// <param name="count"></param>
        /// <param name="outputList"></param>
        /// <typeparam name="T"></typeparam>
        public static void GetOutputNodesConnectedToArrayPort<T>(this Node node, string fieldName, int count, List<T> outputList)
        {
            //apparently, xnode keeps separate ports for each array entry.
            //with arrname 0, arrname 1 etc. 
            for (int i = 0; i < count; i++)
            {
                var fieldNameForIndex = GetFieldNameForArrIndex(fieldName, i);
                var portConnection = node.GetOutputPort(fieldNameForIndex).Connection;
                // Debug.Log($"portConnection { portConnection }");
                if (portConnection != null)
                {
                    // Debug.Log($"portConnection.node { portConnection.node }");
                    
                    if (portConnection.node is T tNode)
                    {
                        outputList.Add(tNode);
                    }
                    else
                    {
                        Debug.LogWarning($"{node} Events port no {i} is connected to {portConnection.node} but doesn't match type {typeof(T)}");
                    }
                }
            }
        }

        public static string GetFieldNameForArrIndex(string fieldName, int index)
        {
            return $"{fieldName} {index}";
        }
    }
}