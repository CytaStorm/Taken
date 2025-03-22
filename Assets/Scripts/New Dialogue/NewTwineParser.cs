using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
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

		//If
		List<string> ifs = GetRemovedSpecialText(currentNode, "\n(if: ", ")");
		List<NewDialogueFlag> flags = ExtractFlags(ifs);

		//Hooks
		List<string> unhookedLinks = 
			GetRemovedSpecialText(currentNode, "[ ", " ]");
		List<NewDialogueLink>[] parsedLinks = new List<NewDialogueLink>[unhookedLinks.Count];

		//Find matching link in currentNode, and add the flag to it
		for (int i = 0; i < flags.Count; i++)
		{
			string unhookedLink = unhookedLinks[i];
			parsedLinks[i] = new List<NewDialogueLink>();

			//break up hooks to find
			List<string[]> splitUnparsedLinks = new List<string[]>();
			foreach (string unparsedLink in GetRemovedSpecialText(unhookedLink, "[[", "]]"))
			{
				splitUnparsedLinks.Add(unparsedLink.Split("->"));

				NewDialogueLink matchingLink = 
					currentNode.Links.FirstOrDefault(
						link => link.Link == splitUnparsedLinks[i][1]);

				matchingLink.Flags.Add(flags[i]);
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
			string[] splitStringToParse = setFlag.Split(' ');

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
		return RemoveSpecialText(node.Text, startDelimiter, endDelimiter);
	}

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
