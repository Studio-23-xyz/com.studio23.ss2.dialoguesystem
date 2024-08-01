using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using Studio23.SS2.DialogueSystem.Data;
using Studio23.SS2.DialogueSystem.Utility;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.CSV;
using UnityEditor.Localization.Plugins.CSV.Columns;
using UnityEditor.Localization.Reporting;
using UnityEngine;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using DialogueGraph = Studio23.SS2.DialogueSystem.Data.DialogueGraph;

namespace Studio23.SS2.Editor.com.studio23.ss2.Editor
{
    public class DialogueGraphExportWindow: EditorWindow
    {
        private DialogueGraph dialogueGraph;
        private List<DialogueLineNodeBase> traversedLinesCache = new();

        private bool ShouldExportGraphs = true;
        private bool ShouldExportSpeakerData = false;
        
        [MenuItem("Studio-23/Dialogue-System/Dialogue Graph Export")]
        public static void ShowWindow()
        {
            GetWindow<DialogueGraphExportWindow>("Dialogue Graph Export");
        }

        private List<DialogueGraph> gs;
        private void OnGUI()
        {
            GUILayout.Label("Dialogue Graph Editor", EditorStyles.boldLabel);

            dialogueGraph = (DialogueGraph)EditorGUILayout.ObjectField("Dialogue Graph", dialogueGraph, typeof(DialogueGraph), false);
            ShouldExportGraphs = EditorGUILayout.Toggle("Should Export Graphs", ShouldExportGraphs);
            ShouldExportSpeakerData = EditorGUILayout.Toggle("Should Export Speaker Data", ShouldExportSpeakerData);
            if (GUILayout.Button("Export graph"))
            {
                ExportSingleGraphToCSV(dialogueGraph);
            }
            if (GUILayout.Button("Export Selected"))
            {
                ExportSelected();
            }
            if (GUILayout.Button("Export ALL"))
            {
                ExportAll();
            }
        }

        private void ExportSelected()
        {
            string folderPath = EditorUtility.OpenFolderPanel("Export graph", "", "");
            if (string.IsNullOrEmpty(folderPath))
                return;
            foreach (var o in Selection.objects)
            {
                if (o is DialogueGraph graph)
                {
                    var filePath = Path.Combine(folderPath , $"{graph.name}.csv");
                    ExportToCSV(graph, filePath);
                }
            }
        }

        private void ExportAll()
        {
            string folderPath = EditorUtility.OpenFolderPanel("Export graph", "", "");
            if (string.IsNullOrEmpty(folderPath))
                return;
            foreach (var graph in GetAllGraphs())
            {
                var filePath = Path.Combine(folderPath , $"{graph.name}.csv");
                ExportToCSV(graph, filePath);
            }
        }

        public List<DialogueGraph> GetAllGraphs()
        {
            List<DialogueGraph> results = new();
            string[] guids = AssetDatabase.FindAssets("t:DialogueGraph", null);
            foreach (string guid1 in guids)
            {
                var graph = AssetDatabase.LoadAssetAtPath<DialogueGraph>(AssetDatabase.GUIDToAssetPath(guid1));
                results.Add(graph);
            }

            return results;
        }
        private void ExportSingleGraphToCSV(DialogueGraph graph)
        {
            string filePath = EditorUtility.SaveFilePanel("Export graph", "", $"{graph.name}.csv", "csv");
            if (string.IsNullOrEmpty(filePath))
                return;
            ExportToCSV(graph, filePath);

        }

        private void ExportToCSV(DialogueGraph graph, string filePath)
        {
            TraverseNodes(graph, HandleDialogueLineNodeTraversed);
            string folderPath = Path.GetDirectoryName(filePath);
            if (ShouldExportGraphs)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            

                if (traversedLinesCache.Count > 0)
                {
                    ExportToCSV(graph,filePath, traversedLinesCache);
                }
            }

            if (ShouldExportSpeakerData)
            {
                var speakerPath = Path.Combine(folderPath, $"{graph.name}_SpeakerData.csv");
                ExportSpeakerData(traversedLinesCache, speakerPath);
            }
        }


        private static SharedTableData.SharedTableEntry GetTableEntry(DialogueLineNodeBase dialogue)
        {
            var table = LocalizationSettings.StringDatabase.GetTableAsync(dialogue.GetLocalizationTable()).Result;

            return table.SharedData.GetEntryFromReference(dialogue.DialogueLocalizedString.TableEntryReference);
        }

        public static void ExportToCSV(DialogueGraph graph, string filePath, List<DialogueLineNodeBase> dialogueLinesInOrder)
        {
            ExportStringTable(graph, filePath, dialogueLinesInOrder);
        }

        private static void ExportStringTable(DialogueGraph graph, string path, List<DialogueLineNodeBase> dialogueLinesInOrder)
        {
            var dialogueGraphLineIds = dialogueLinesInOrder
                .Where(l=> GetTableEntry(l) != null)
                .Select(l => GetTableEntry(l).Id)
                .ToHashSet();
            
            // var csvColumnsList = ColumnMapping.CreateDefaultMapping();
            var columnMappings = new List<CsvColumns>();
            columnMappings.Add(new KeyIdColumns
            {
                IncludeId = true, // Include the Id column field.
                IncludeSharedComments = false, // Include Shared comments.
            });

            foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
            {
                // Export English with no comments
                columnMappings.Add(new LocaleColumns
                {
                    LocaleIdentifier = locale.Identifier,
                    IncludeComments = false
                });
            }
            if (!graph.TryGetDefaultTable(out var tableReference))
            {
                return;
            }
            // CsvWriter
            // tableReference.TableCollectionName;
            var collection =  LocalizationEditorSettings.GetStringTableCollection(tableReference.TableCollectionName);
            using (var stream = new StreamWriter(path, false, new UTF8Encoding(false)))
            {
                ExportStringTable(stream, collection, columnMappings, dialogueLinesInOrder, dialogueGraphLineIds);
            }   
        }

        public void ExportSpeakerData(List<DialogueLineNodeBase> dialogueLinesInOrder, string path)
        {
            // StringBuilder to construct the CSV content
            StringBuilder csvContent = new StringBuilder();

            // Append the header
            csvContent.AppendLine("Key,Id,Speaker, Expression");

            // Append each dialogue line
            foreach (var line in dialogueLinesInOrder)
            {
                var entry = GetTableEntry(line);
                if (entry != null)
                {
                    if (line.SpeakerData.Character != null)
                    {
                        csvContent.AppendLine($"{entry.Key},{entry.Id},{line.SpeakerData.Character.CharacterName}, {line.SpeakerData.Expression}");
                    }
                    else
                    {
                        csvContent.AppendLine($"{entry.Key},{entry.Id},,");
                    }
                }
            }

            // Write the content to the file
            File.WriteAllText(path, csvContent.ToString());
        }
        
        /// <summary>
        /// Basically a clone of Unity's CSV Exporter
        /// We need to mimic it because our exported csv's
        /// Escaped dialogue strings and csv format must follow whatever unity does
        /// #NOTE this uses CSVHelper. Unity may choose another lib or update their code
        /// In that case this function needs to be updated
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="allowedIds"></param>
        /// <param name="collection"></param>
        /// <param name="columnMappings"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ExportStringTable(TextWriter writer, StringTableCollection collection, IList<CsvColumns> columnMappings, List<DialogueLineNodeBase> dialigueLinesInOrder, HashSet<long> allowedIds)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                try
                {
                    Debug.Log("Writing Headers");
                    foreach (var cell in columnMappings)
                    {
                        cell.WriteBegin(collection, csvWriter);
                    }

                    //---------------------------------CUSTOM CODE---------------------------------
                    //this is the only new part
                    //we only want to print the rows for the given graph
                    //hence check if the id is in the hashset
                    //Then ensure same order as dialoguelines
                    //This could be optimized by looping over the dialoguelines
                    //but I'll prioritize easier upgrade with unity version for now
                    Dictionary<long, int> IdToOrderMap = new();
                    for (var i = 0; i < dialigueLinesInOrder.Count; i++)
                    {
                        var dialogueLineNodeBase = dialigueLinesInOrder[i];
                        var entry = GetTableEntry(dialogueLineNodeBase);
                        if (entry != null)
                        {
                            IdToOrderMap.TryAdd(entry.Id, i);
                        }
                    }

                    // collection.GetRowEnumerator() returns a row object that is cached
                    // every iteration of foreach it reuses the same object
                    // We can't just run LINQ on it. 
                    // IF we do the lonq result contains the last value of the reused obejct in every index...
                    // so we make a clone list
                    List<(SharedTableData.SharedTableEntry KeyEntry, StringTableEntry[] TableEntries)> rows = new();
                    foreach (var row in collection.GetRowEnumerator())
                    {
                        if (row.TableEntries[0] != null && row.TableEntries[0].SharedEntry.Metadata.HasMetadata<ExcludeEntryFromExport>())
                            continue;
                        
                        if (allowedIds.Contains(row.KeyEntry.Id))
                        {
                            rows.Add((row.KeyEntry, row.TableEntries.Clone() as StringTableEntry[]));
                        }
                    }

                    rows.Sort(
                        (r1, r2) => IdToOrderMap[r1.KeyEntry.Id].CompareTo(IdToOrderMap[r2.KeyEntry.Id])
                    );
                    Debug.Log($"Writing Contents: {rows.Count()} lines");
                    foreach (var row in rows)
                    {
                        csvWriter.NextRecord();
                        foreach (var cell in columnMappings)
                        {
                            if (string.IsNullOrEmpty(row.KeyEntry.Key))
                                continue;
                                
                            cell.WriteRow(row.KeyEntry, row.TableEntries, csvWriter);
                        }
                    }

                    foreach (var cell in columnMappings)
                    {
                        cell.WriteEnd(collection);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Failed Exporting.\n" + e.Message);
                    throw;
                }
            }
        }

        private static void WriteCSV(List<DialogueLineNodeBase> dialogueLines, string path)
        {
            StringBuilder csvContent = new StringBuilder();

            // Append the header
            csvContent.AppendLine("Key,Id,English(en),Japanese(ja),Russian(ru),Spanish(es),Bangla(bn)");

            // Append each dialogue line
            foreach (var line in dialogueLines)
            {
                var entry = GetTableEntry(line);
                if (entry == null)
                {
                    Debug.LogError($"entry null for {line}", line);
                }
                else
                {
                    csvContent.AppendLine($"{entry.Key},{entry.Id},{line.DialogueLocalizedString.GetLocalizedStringInEditor()},,,,");
                }
                // csvContent.AppendLine($"{line.DialogueLocalizedString.Keys},{line.Key},{line.English},{line.Japanese},{line.Russian},{line.Spanish},{line.Bangla}");
            }

            // Write the content to the file
            File.WriteAllText(path, csvContent.ToString());
        }

        public void HandleDialogueLineNodeTraversed(DialogueLineNodeBase node)
        {
            traversedLinesCache.Add(node);
        }

        public void TraverseNodes(DialogueGraph graph, Action<DialogueLineNodeBase> handleLineNodeEncountered)
        {
            List<DialogueStartNode> startNodes = graph.nodes.OfType<DialogueStartNode>().ToList();
            HashSet<DialogueNodeBase> encounteredNodes = new();
            traversedLinesCache.Clear();
            foreach (var startNode in startNodes)
            {
                TraverseNodes(startNode,encounteredNodes,handleLineNodeEncountered);
            }
        }
        
        public void TraverseNodes(DialogueNodeBase startNode, HashSet<DialogueNodeBase> encounteredNodes, 
            Action<DialogueLineNodeBase> handleLineNodeEncountered)
        {
            DialogueNodeBase curNode = startNode;
            while (curNode != null)
            {
                if (!encounteredNodes.Add(curNode))
                {
                    //the dialoguge graph can loop
                    //hence we need to check if  we have encountered the node before
                    return;
                }
                if (curNode is DialogueLineNodeBase dialogueLineNodeBase)
                {
                    //Note: this can be a dialoguechoiceNode. Doesn't matter as we want only the line from there.
                    // Debug.Log("dialogueLineNodeBase " + dialogueLineNodeBase.DialogueLocalizedString.GetLocalizedStringInEditor());
                    handleLineNodeEncountered(dialogueLineNodeBase);
                }else if (curNode is DialogueChoicesNode choicesNode)
                {
                    var choiceNodes = choicesNode.FetchAllConnectedChoiceNodes();
                    foreach (var node in choiceNodes)
                    {
                        //if we encounter a dialogue choices node, recursively call
                        // so that we traverse the entire chain before changing chains
                        TraverseNodes(node, encounteredNodes, handleLineNodeEncountered);
                    }
                    //#todo add THIS TO FetchAllConnectedChoiceNodes()
                    var forceExitNode = choicesNode.GetForceExitNode();
                    if (forceExitNode != null)
                    {
                        TraverseNodes(forceExitNode, encounteredNodes, handleLineNodeEncountered);
                    }
                }

                //for choices branches, we've already explored all branches at this point
                //so don't bother
                if (curNode is not DialogueChoicesNode)
                {
                    
                    curNode = curNode.GetNextNode();
                }
            }
        }
    }
}


