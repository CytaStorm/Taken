using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TwineParser
{
	/// <summary>
	/// Parses JSON Passage
	/// </summary>
	/// <param name="jsonPassage"></param>
	/// <param name="passagesLeft"></param>
	/// <returns></returns>
	public static void ParseNode(DialogueNode node)
	{
		//Parse link conditions
		ParseLinkConditions(node);
		//Parse flags that entry to this node causes
		AddNewChangeFlags(node);

		//Clear any remaining links
		RemoveNodeSpecialText(node, "\n[[", "]]");
		return;
	}

	private static void ParseNewNode(DialogueNode currentNode)
	{
		//parse flags
		
		//These are used in twinery, but not here
		//RemoveSpecialText(currentNode, "(if:", "]\n");
		//RemoveSpecialText(currentNode, "(else-if:", "]\n");
		//RemoveSpecialText(currentNode, "(else:", "]\n");


		//Parse speaker of dialogue
		//ParseTextFormat(currentNode);

		//Trim anything leftover
		//currentNode.Info = currentNode.Info.Trim();
	}

	/// <summary>
	/// Parses Twine's if statements to link nodes
	/// </summary>
	/// <param name="currentNode">Node being parsed</param>
	private static void ParseLinkConditions(DialogueNode currentNode)
	{
		//Get ifs and their hooks, these should be the same length

		//If
		List<string> ifs = GetNodeRemovedSpecialText(currentNode, "\n(if: ", ")");
		
		//Each item in this list is a list of dialogueflags representing
		//the flags in each link condition block (if: ... )
		//
		//The flags list should be the same length as the amount if 
		//ifs there are in the node
		List<List<DialogueFlag>> flags = ExtractFlags(ifs);

		//Hooks
		List<string> unhookedLinks = 
			GetNodeRemovedSpecialText(currentNode, "[ ", " ]");
		List<DialogueLink>[] parsedLinks = new List<DialogueLink>[unhookedLinks.Count];

		//Find matching link in currentNode, and add the flag to it
		for (int i = 0; i < flags.Count; i++)
		{
			string unhookedLink = unhookedLinks[i];
			//parsedLinks[i] = new List<NewDialogueLink>();

			//break up hooks to find
			List<string[]> splitUnparsedLinks = new List<string[]>();

			List<string> unparsedLinks = GetRemovedSpecialText(unhookedLink, "[[", "]]");
			for (int unparsedLinkCounter = 0; unparsedLinkCounter < unparsedLinks.Count; unparsedLinkCounter++)
			{
				string unparsedLink = unparsedLinks[unparsedLinkCounter];
				splitUnparsedLinks.Add(unparsedLink.Split("->"));

				DialogueLink matchingLink = 
					currentNode.Links.FirstOrDefault(
						link => link.Link == splitUnparsedLinks[unparsedLinkCounter][1]);

				matchingLink.Flags.AddRange(flags[i]);
			}
		}

		//UNDER CONSTRUCTION, PARSE HOOKS INTO LINKS
		//List<NewDialogueLink>[] parsedLinks = new List<NewDialogueLink>[parsedLinks.Length];

		////Find and update matching link in current node
		//foreach (List<NewDialogueLink> hook in parsedLinks)
		//{
		//	foreach (string link in hook)
		//	{
		//		NewDialogueLink matchingLink = currentNode.Links.FirstOrDefault(
		//			match => match.);
		//	}
		//}
	}

	/// <summary>
	/// Sets all flags to be changed upon node entry.
	/// </summary>
	/// <param name="currentNode">Node being parsed.</param>
	private static void AddNewChangeFlags(DialogueNode currentNode)
	{
		//Get the string to parse
		List<string> stringToParse = 
			GetNodeRemovedSpecialText(currentNode, "\n(set: ", ")");
		//If it doesn't have (set:) return
		if (stringToParse.Count == 0) return;

		//ExtractFlags returns a list of list of newdialogue flags, but because each
		//twine set command only edits one variable, we can assume ExtractFlag's
		//return list has only one element
		currentNode.FlagsToChange.AddRange(ExtractFlags(stringToParse)[0]);
		string test = currentNode.FlagsToChange[0].Name;
	}

	/// <summary>
	/// Parses extracts set flag text.
	/// </summary>
	/// <param name="stringsToParse">List of extracted set flag statements</param>
	/// <returns></returns>
	private static List<List<DialogueFlag>> ExtractFlags (List<string> stringsToParse)
	{
		List<List<DialogueFlag>> result = new List<List<DialogueFlag>>();
		foreach (string setFlag in stringsToParse)
		{
			//Break up ANDS
			string[] splitFlag = setFlag.Split(" and ");

			//Extract Flags
			List<DialogueFlag> extractedFlags = ParseFlags(splitFlag);

			result.Add(extractedFlags);
		}
		return result;
	}

	/// <summary>
	/// Parses a string array of twine commands to
	/// a list of DialogueFlags, representing the change to
	/// existing flags.
	/// </summary>
	/// <param name="splitFlag">Twine commands to parse.</param>
	/// <returns>List of parsed DialogueFlags</returns>
	private static List<DialogueFlag> ParseFlags(string[] splitFlag)
	{
		List<DialogueFlag> result = new List<DialogueFlag>();
		foreach (string flag in splitFlag)
		{
			//Parse it
			string[] splitStringToParse = flag.Split(' ');


			//substring starting at 1 to get rid of dollar sign
			string flagName = splitStringToParse[0].Substring(1);

			//Differentiate between value and bool
			//Bool flag
			if (splitStringToParse[1] == "to" || splitStringToParse[1] == "is")
            {
                ParseBoolFlag(result, splitStringToParse, flagName);
            }

            //Value flag
            else
            {
                ParseValueFlag(result, splitStringToParse, flagName);
            }
        }
		return result;

		//Parse Bool Flags
        static void ParseBoolFlag(
			List<DialogueFlag> result, string[] splitStringToParse, string flagName)
        {
            bool flagTruthness;

            if (splitStringToParse[2] == "true")
            {
                flagTruthness = true;
            }
            else
            {
                flagTruthness = false;
            }

			DialogueFlagBool test = new DialogueFlagBool(flagName, flagTruthness);
			string testName = test.Name;
            result.Add(new DialogueFlagBool(flagName, flagTruthness));
        }

		//Parse Value Flags
        static void ParseValueFlag(
			List<DialogueFlag> result, string[] splitStringToParse, string flagName)
        {
            int valueRightHandSide = int.Parse(splitStringToParse[2]);

            //increment/decrement
            switch (splitStringToParse[1])
            {
                //Increment/decrement
                case "-=":
                case "+=":
                    result.Add(new DialogueFlagValue(valueRightHandSide, flagName));
                    break;
                //Direct setting
                case "to":
                    result.Add(new DialogueFlagValue(flagName, valueRightHandSide));
                    break;

                //exclusively for comparison with ifs
                //(if: $test > 5)
                case "=":
					result.Add(
						new DialogueFlagValue(
							flagName, valueRightHandSide, ValueRelation.Equal));
                    break;
                case ">":
					result.Add(
						new DialogueFlagValue(
							flagName, valueRightHandSide, ValueRelation.Greater));
                    break;
                case "<":
					result.Add(
						new DialogueFlagValue(
							flagName, valueRightHandSide, ValueRelation.Less));
                    break;
                default:
                    throw new ArgumentException("Bad parse");
            }
        }
    }

    #region String operations
	/// <summary>
	/// Returns list of removed text in node between delimiters.
	/// </summary>
	/// <param name="inputString">Input to edit on.</param>
	/// <param name="startDelimiter">Start delimiter to look for.</param>
	/// <param name="endDelimiter">End delimiter to look for.</param>
	/// <returns>Returns list of removed text in node between 
	/// delimiters.</returns>
	private static List<string> GetRemovedSpecialText(
		string inputString, string startDelimiter, string endDelimiter)
	{
		List<string> result = new List<string>();
		foreach (
			(string text, int) removedTextTuple in 
			RemoveSpecialText(inputString, startDelimiter, endDelimiter))
		{
			result.Add(removedTextTuple.text);
		};
		return result;
	}

	/// <summary>
    /// Removes text between two specified delimiters, along with the delimiters.
    /// </summary>
    /// <param name="inputString">Input you want to edit on.</param>
    /// <param name="startDelimiter">Start delimiter to start deleting at.</param>
    /// <param name="endDelimiter">End delimiter to end deletion at.</param>
    /// <returns>List of tuples, containing the removed text (excluding delimiters)
    /// and the starting index of that removed text (including delimiters)</returns>
    private static List<(string specialText, int removedStartIndex)> RemoveSpecialText(
		string inputString, string startDelimiter, string endDelimiter)
	{
		List<(string specialText, int removedStartIndex)> results = 
			new List<(string specialText, int removedStartIndex)>();
		int startDelimiterStartIndex = inputString.IndexOf(startDelimiter);
		while (startDelimiterStartIndex != -1)
		{
			//Find first set of delimiters
			int specialTextStartIndex = 
				startDelimiterStartIndex + startDelimiter.Length;

			int specialTextEndIndex = inputString.IndexOf(endDelimiter, specialTextStartIndex + 1);

			int specialTextLength = specialTextEndIndex - specialTextStartIndex;

			string specialText = inputString.Substring(
				specialTextStartIndex, specialTextLength);
			results.Add((specialText, startDelimiterStartIndex));

			inputString = inputString.Remove(
				specialTextStartIndex - startDelimiter.Length,
				specialText.Length +
				startDelimiter.Length +
				endDelimiter.Length);

			startDelimiterStartIndex = inputString.IndexOf(startDelimiter);
		}
		return results;
	}

	#endregion

    #region Node operations
    /// <summary>
    /// Returns list of removed text in node between delimiters.
    /// </summary>
    /// <param name="node">Node to edit on.</param>
    /// <param name="startDelimiter">Start delimiter to look for.</param>
    /// <param name="endDelimiter">End delimiter to look for.</param>
    /// <returns>Returns list of removed text in node between 
    /// delimiters.</returns>
    private static List<string> GetNodeRemovedSpecialText(
		DialogueNode node, string startDelimiter, string endDelimiter)
	{
		List<string> result = new List<string>();
		foreach (
			(string text, int) removedTextTuple in 
			RemoveNodeSpecialText(node, startDelimiter, endDelimiter))
		{
			result.Add(removedTextTuple.text);
		};
		return result;
	}

	/// <summary>
	/// Removes text between two specified delimiters, along with the delimiters. Used with Nodes.
	/// </summary>
	/// <param name="node">Node you want to edit on.</param>
	/// <param name="startDelimiter">Start delimiter to start deleting at.</param>
	/// <param name="endDelimiter">End delimiter to end deletion at.</param>
	/// <returns>List of tuples, containing the removed text (excluding delimiters)
	/// and the starting index of that removed text (including delimiters)</returns>
	private static List<(string specialText, int removedStartIndex)> RemoveNodeSpecialText(
		DialogueNode node, string startDelimiter, string endDelimiter)
	{
		List<(string specialText, int removedStartIndex)> results = 
			new List<(string specialText, int removedStartIndex)>();
		int startDelimiterStartIndex = node.Text.IndexOf(startDelimiter);
		while (startDelimiterStartIndex != -1)
		{
			//Find first set of delimiters
			int specialTextStartIndex = 
				startDelimiterStartIndex + startDelimiter.Length;

			int specialTextEndIndex = node.Text.IndexOf(endDelimiter, specialTextStartIndex + 1);

			int specialTextLength = specialTextEndIndex - specialTextStartIndex;

			string specialText = node.Text.Substring(
				specialTextStartIndex, specialTextLength);
			results.Add((specialText, startDelimiterStartIndex));

			node.Text = node.Text.Remove(
				specialTextStartIndex - startDelimiter.Length,
				specialText.Length +
				startDelimiter.Length +
				endDelimiter.Length);

			startDelimiterStartIndex = node.Text.IndexOf(startDelimiter);
		}
		return results;
	}
	#endregion
}
