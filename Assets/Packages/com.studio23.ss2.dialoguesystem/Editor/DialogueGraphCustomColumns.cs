using System;
using System.Collections.Generic;
using CsvHelper;
using Studio23.SS2.DialogueSystem.Data;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.CSV.Columns;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Tables;


/// <summary>
/// Writes custom DialogueGraph Fields <see cref="StringTable"/>.
/// </summary>
[Serializable]
public class DialogueGraphCustomColumns : CsvColumns, ISerializationCallbackReceiver
{
    private Dictionary<long, DialogueLineNodeBase> IdToDialogueLineMap;

    private List<string> ColNames = new()
    {
        "SpeakerName",
        "Expression"
    };

    public DialogueGraphCustomColumns(Dictionary<long, DialogueLineNodeBase> idToDialogueLineMap)
    {
        IdToDialogueLineMap = idToDialogueLineMap;
    }

    /// <inheritdoc/>
    public override void WriteBegin(StringTableCollection collection, CsvWriter csvWriter)
    {
        var tables = collection.StringTables;

        foreach (var colName in ColNames)
        {
            csvWriter.WriteField(colName);
        }
    }

    /// <inheritdoc/>
    public override void ReadBegin(StringTableCollection collection, CsvReader csvReader)
    {
        //do nothing
    }

    /// <inheritdoc/>
    public override void ReadEnd(StringTableCollection collection)
    {
        //do nothing
    }

    /// <inheritdoc/>
    public override void ReadRow(SharedTableData.SharedTableEntry keyEntry, CsvReader reader)
    {
        //do nothing
    }

    /// <inheritdoc/>
    public override void WriteRow(SharedTableData.SharedTableEntry keyEntry, IList<StringTableEntry> tableEntries,
        CsvWriter writer)
    {
        if (IdToDialogueLineMap.TryGetValue(keyEntry.Id, out var dialogueLineNodeBase))
        {
            if (dialogueLineNodeBase.SpeakerData.Character != null)
            {
                writer.WriteField(dialogueLineNodeBase.SpeakerData.Character.CharacterName);
            }
            else
            {
                writer.WriteField("");
            }

            if (dialogueLineNodeBase.SpeakerData.Expression != null)
            {
                writer.WriteField(dialogueLineNodeBase.SpeakerData.Expression.ExpressionName);
            }
            else
            {
                writer.WriteField("");
            }
        }
        else
        {
            //write empty
            foreach (var colName in ColNames)
            {
                writer.WriteField("");
            }
        }
    }

    /// <summary>
    /// Sets the field names to their default values.
    /// The default values are <see cref="LocaleIdentifier.ToString"/> for <see cref="FieldName"/>
    /// and <see cref="FieldName"/> + " Comments" for <see cref="CommentFieldName"/>.
    /// </summary>
    public void OnBeforeSerialize()
    {
        //do nothing
    }

    public void OnAfterDeserialize()
    {
        //do nothing
    }
}