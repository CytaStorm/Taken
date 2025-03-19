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
		return;
	}

	private static void ParseNewNode(NewDialogueNode currentNode)
	{
		//parse flags
		
		//These are used in twinery, but not here
		//RemoveSpecialText(currentNode, "(if:", "]\n");
		//RemoveSpecialText(currentNode, "(else-if:", "]\n");
		//RemoveSpecialText(currentNode, "(else:", "]\n");

		//Parse flags that entry to this node causes
		ParseNewChangeFlags(currentNode);

		//Parse speaker of dialogue
		//ParseTextFormat(currentNode);

		//Trim anything leftover
		//currentNode.Info = currentNode.Info.Trim();
	}

	private static void ParseNewChangeFlags(NewDialogueNode currentNode)
	{
		//Get the string to parse
		List<string> stringToParse = 
			GetRemovedSpecialText(currentNode, "(set: ", ")");
		//If it doesn't have (set:) return
		if (stringToParse.Count == 0) return;

		//Parse it
		string[] splitStringToParse = stringToParse[0].Split(' ');
		bool changeFlagTo;
		if (splitStringToParse[2] == "true") 
		{
			changeFlagTo = true; 
		} 
		else 
		{
			changeFlagTo = false; 
		}
		currentNode.FlagsToChange.Add(
			new DialogueFlag(splitStringToParse[0], changeFlagTo));
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
			int specialTextEndIndex = -1000;

			//In case they are the same, search for next instance of delimiter
			if (startDelimiter == endDelimiter)
			{
				specialTextEndIndex = node.Text.IndexOf(
				endDelimiter, node.Text.IndexOf(endDelimiter) + 1);
			}
			else
			{
				specialTextEndIndex = node.Text.IndexOf(endDelimiter);
			}

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
}
