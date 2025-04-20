using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

		//Clear any remaining links
		RemoveSpecialText(node, "\n[[", "]]");
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
		List<List<NewDialogueFlag>> flags = ExtractFlags(ifs);

		//Hooks
		List<string> unhookedLinks = 
			GetRemovedSpecialText(currentNode, "[ ", " ]");
		List<NewDialogueLink>[] parsedLinks = new List<NewDialogueLink>[unhookedLinks.Count];

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

				NewDialogueLink matchingLink = 
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
	private static void AddNewChangeFlags(NewDialogueNode currentNode)
	{
		//Get the string to parse
		List<string> stringToParse = 
			GetRemovedSpecialText(currentNode, "\n(set: ", ")");
		//If it doesn't have (set:) return
		if (stringToParse.Count == 0) return;

		//ExtractFlags returns a list of list of newdialogue flags, but because each
		//twine set command only edits one variable, we can assume ExtractFlag's
		//return list has only one element
		currentNode.FlagsToChange.AddRange(ExtractFlags(stringToParse)[0]);
	}

	/// <summary>
	/// Parses extracts set flag text.
	/// </summary>
	/// <param name="stringsToParse">List of extract set flag statements</param>
	/// <returns></returns>
	private static List<List<NewDialogueFlag>> ExtractFlags (List<string> stringsToParse)
	{
		List<List<NewDialogueFlag>> result = new List<List<NewDialogueFlag>>();
		foreach (string setFlag in stringsToParse)
		{
			//Break up ANDS
			string[] splitFlag = setFlag.Split(" and ");

			List<NewDialogueFlag> extractedFlag = new List<NewDialogueFlag>();
			//default to flase
			bool flagTruthness = false;

			foreach (string flag in splitFlag)
			{
				//Parse it
				string[] splitStringToParse = flag.Split(' '); 

				if (splitStringToParse[2] == "true")
				{
					flagTruthness = true;
				} else
				{
					flagTruthness = false;
				}

				//substring starting at 1 to get rid of dollar sign
				extractedFlag.Add(new NewDialogueFlag(splitStringToParse[0].Substring(1), flagTruthness));
			}
			result.Add(extractedFlag);
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
