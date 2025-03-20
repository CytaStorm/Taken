using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTwineParser
{
	/// <summary>
	/// Parses JSON Passage
	/// </summary>
	/// <param name="jsonPassage"></param>
	/// <param name="passagesLeft"></param>
	/// <returns></returns>
	public static void ParseNode(NewDialogueNode node)
	{
		//Parse link conditions
		ParseLinkConditions(node);
		//Parse flags that entry to this node causes
		AddNewChangeFlags(node);

		return;
	}

	private static void ParseNewNode(NewDialogueNode currentNode)
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
	private static void ParseLinkConditions(NewDialogueNode currentNode)
	{
		//Get ifs and their hooks, these should be the same length
		List<string> ifs = GetRemovedSpecialText(currentNode, "\n(if:", ")");
		List<string> hooks = GetRemovedSpecialText(currentNode, "[", "]");
		
		//parse hooks first
	}

	/// <summary>
	/// Sets all flags to be changed upon node entry.
	/// </summary>
	/// <param name="currentNode">Node being parsed.</param>
	private static void AddNewChangeFlags(NewDialogueNode currentNode)
	{
		//Get the string to parse
		List<string> stringToParse = 
			GetRemovedSpecialText(currentNode, "\n(set: ", ")");
		//If it doesn't have (set:) return
		if (stringToParse.Count == 0) return;
		
		currentNode.FlagsToChange.AddRange(ExtractFlags(stringToParse));
	}

	/// <summary>
	/// Parses extracts set flag text.
	/// </summary>
	/// <param name="stringsToParse">List of extract set flag statements</param>
	/// <returns></returns>
	private static List<NewDialogueFlag> ExtractFlags (List<string> stringsToParse)
	{
		List<NewDialogueFlag> result = new List<NewDialogueFlag>();
		foreach (string setFlag in stringsToParse)
		{
			//Parse it
			string[] splitStringToParse = stringsToParse[0].Split(' ');
			bool changeFlagTo;
			if (splitStringToParse[2] == "true") 
			{
				changeFlagTo = true; 
			} 
			else 
			{
				changeFlagTo = false; 
			}
			result.Add(
				new NewDialogueFlag(splitStringToParse[0].Substring(1), changeFlagTo));
		}
		return result;
	}

	/// <summary>
	/// Removes text between two specified delimiters, along with the delimiters.
	/// </summary>
	/// <param name="node">Node you want to edit on.</param>
	/// <param name="startDelimiter">Start delimiter to start deleting at.</param>
	/// <param name="endDelimiter">End delimiter to end deletion at.</param>
	/// <returns>List of tuples, containing the removed text (excluding delimiters)
	/// and the starting index of that removed text (including delimiters)</returns>
	private static List<(string specialText, int removedStartIndex)> RemoveSpecialText(
		NewDialogueNode node, string startDelimiter, string endDelimiter)
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

	/// <summary>
	/// Returns list of removed text in node between delimiters.
	/// </summary>
	/// <param name="node">Node to edit on.</param>
	/// <param name="startDelimiter">Start delimiter to look for.</param>
	/// <param name="endDelimiter">End delimiter to look for.</param>
	/// <returns>Returns list of removed text in node between 
	/// delimiters.</returns>
	private static List<string> GetRemovedSpecialText(
		NewDialogueNode node, string startDelimiter, string endDelimiter)
	{
		List<string> result = new List<string>();
		foreach (
			(string text, int) removedTextTuple in 
			RemoveSpecialText(node, startDelimiter, endDelimiter))
		{
			result.Add(removedTextTuple.text);
		};
		return result;
	}

}
