using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


class DialogueTreeParser : MonoBehaviour
{
	/// <summary>
	/// Parse twine file.
	/// </summary>
	/// <param name="textFile">File to parse.</param>
	/// <returns>List of nodes parsed from the twine file.</returns>
	public static List<DialogueNode> ParseFile(TextAsset textFile)
    {
        // Makes sure the file exists
        // Makes a list for the dialogue nodes
        List<DialogueNode> dialogueNodes = new List<DialogueNode>();
        string[] fileLines = textFile.text.Split('\n');

		// FIRST TIME THROUGH -- create nodes and their data
        DialogueNode currentNode = null;
		for (int i = 0; i < fileLines.Length; i++)
		{
			string line = fileLines[i].Trim();

			//Line is not a node starter line
			if (!line.StartsWith("::"))
			{
				//If no node is being made, ignore.
				if (currentNode == null) continue;

				// Adds the info under the node
				currentNode.Info += line + "\n";
				continue;
			}

			//Filter out metadata/bad lines
			string nodeName = ParseNodeName(line);
			if (nodeName == "StoryTitle" || nodeName == "StoryData") continue;

			//Parse special text once node is completed
			if (currentNode != null)
			{
				ParseSpecialText(currentNode);
			}

			//Only new nodes should be left over
			//Start new node
			currentNode = new DialogueNode(nodeName, "");
			dialogueNodes.Add(currentNode);
		}

		//Parse specialText in lastNode 
		ParseSpecialText(currentNode);

		//Create the links between nodes
		foreach (DialogueNode node in dialogueNodes)
		{
			if (node.LinkNames == null) continue;

			foreach (string linkName in node.LinkNames)
			{
				FindMatchingNode(dialogueNodes, node, linkName);
			}
		}

		return dialogueNodes;
    }

	/// <summary>
	/// Parses special delimiters in the node.
	/// </summary>
	/// <param name="currentNode">Node to parse.</param>
	private static void ParseSpecialText(DialogueNode currentNode)
	{
		//Links
		currentNode.LinkNames = GetRemovedSpecialText(currentNode, "[[", "]]");

		//Add flags (for conditions required to enter node)
		AddFlags(currentNode);
		
		//Remove flag delimiters (for conditions required to enter node)
		//These are used in twinery, but not here
		RemoveSpecialText(currentNode, "(if:", "]\n");
		RemoveSpecialText(currentNode, "(else-if:", "]\n");
		RemoveSpecialText(currentNode, "(else:", "]\n");

		//Parse flags that entry to this node causes
		ParseChangeFlags(currentNode);

		//Parse speaker of dialogue
		ParseTextFormat(currentNode);

		//Trim anything leftover
		currentNode.Info = currentNode.Info.Trim();
	}

	/// <summary>
	/// Parse the Node Name.
	/// </summary>
	/// <param name="trimmedLine">Line to trim the name of the node from.</param>
	/// <returns>The name of the node.</returns>
	private static string ParseNodeName(string trimmedLine)
	{
		int braceIndex = trimmedLine.IndexOf("{");
		string nodeName;
		if (braceIndex > -1)
		{
			nodeName = trimmedLine.Substring(2, braceIndex - 2).Trim();
		} 
		else
		{
			nodeName = trimmedLine.Substring(2).Trim();
		}
		return nodeName;
	}

	/// <summary>
	/// Adds node with matching name to a specified node's links list.
	/// </summary>
	/// <param name="dialogueNodes">List of nodes to look through.</param>
	/// <param name="node">The node you want to add the link to.</param>
	/// <param name="linkName">The name of the node you want to add.</param>
	private static void FindMatchingNode(
		List<DialogueNode> dialogueNodes, DialogueNode node, string linkName)
	{
		foreach (DialogueNode innerNode in dialogueNodes)
		{
			//See if there's a continue button node
			try
			{
				if (innerNode.NodeName == linkName.Split("->")[1])
				{
					node.Links.Add(innerNode);
					innerNode.NodeName = "Continue";
					break;
				}
			}
			catch (IndexOutOfRangeException e)
			{
				//There is no continue button
			}

			// check to see if any continues are in there
			if (innerNode.NodeName == linkName)
			{
				node.Links.Add(innerNode);
				//print("matching node name: " + node.Links[node.Links.Count - 1].NodeName);
				break;
			}
		}
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
		DialogueNode node, string startDelimiter, string endDelimiter)
	{
		List<(string specialText, int removedStartIndex)> results = 
			new List<(string specialText, int removedStartIndex)>();
		int startDelimiterStartIndex = node.Info.IndexOf(startDelimiter);
		while (startDelimiterStartIndex != -1)
		{
			//Find first set of delimiters
			int specialTextStartIndex = 
				startDelimiterStartIndex + startDelimiter.Length;
			int specialTextEndIndex = -1000;

			//In case they are the same, search for next instance of delimiter
			if (startDelimiter == endDelimiter)
			{
				specialTextEndIndex = node.Info.IndexOf(
				endDelimiter, node.Info.IndexOf(endDelimiter) + 1);
			}
			else
			{
				specialTextEndIndex = node.Info.IndexOf(endDelimiter);
			}

			int specialTextLength = specialTextEndIndex - specialTextStartIndex;

			string specialText = node.Info.Substring(
				specialTextStartIndex, specialTextLength);
			results.Add((specialText, startDelimiterStartIndex));

			node.Info = node.Info.Remove(
				specialTextStartIndex - startDelimiter.Length,
				specialText.Length +
				startDelimiter.Length +
				endDelimiter.Length);

			startDelimiterStartIndex = node.Info.IndexOf(startDelimiter);
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
		DialogueNode node, string startDelimiter, string endDelimiter)
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

	/// <summary>
	/// Parses the flags to change in upon node entry.
	/// </summary>
	/// <param name="node">Node to edit.</param>
	private static void ParseChangeFlags(DialogueNode node)
	{
		//Get the string to parse
		List<string> stringToParse = 
			GetRemovedSpecialText(node, "(set: ", ")");
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
		node.FlagsToChange.Add(
			new DialogueFlag(splitStringToParse[0], changeFlagTo));
	}

	/// <summary>
	/// Adds condition requirements to enter node to node.
	/// </summary>
	/// <param name="node">Node to edit.</param>
	private static void AddFlags(DialogueNode node)
	{
		//Trues
		foreach (string trues in 
			GetRemovedSpecialText(node, "<<", ">>"))
		{
			node.Flags.Add(new DialogueFlag(trues, true));
		}

		//Falses
		foreach (string falses in 
			GetRemovedSpecialText(node, "``", "``"))
		{
			node.Flags.Add(new DialogueFlag(falses, false));
		}
	}

	/// <summary>
	/// Parses text formatting for names and italics and such.
	/// </summary>
	/// <param name="node">Node to edit.</param>
	private static void ParseTextFormat(DialogueNode node)
	{
		//Bold and fancy text name
		string speakerFormatStartDelimiter = "<b><font=SpeakerFont>";
		string speakerFormatEndDelimiter = "</font></b>";
		List<(string, int)> speakerSpecialTextTuple =
			RemoveSpecialText(node, "\'\'", "\'\'");

		int formattedSpeakerNameOffset = 0;
		for (int i = 0; i < speakerSpecialTextTuple.Count; i++)
		{
			(string text, int index) removedTextTuple = 
				speakerSpecialTextTuple[i];

			string formattedSpeakerName = 
				speakerFormatStartDelimiter + 
				removedTextTuple.text + 
				speakerFormatEndDelimiter;
			node.Info = node.Info.Insert(
				removedTextTuple.index + formattedSpeakerNameOffset,
				formattedSpeakerName);
			formattedSpeakerNameOffset += formattedSpeakerName.Length;
		}

		//Italics
		foreach ((string text, int index) removedTextTuple in
			RemoveSpecialText(node, "//", "//"))
		{
			node.Info = 
				node.Info.Insert(
					removedTextTuple.index, 
					"<i>" + removedTextTuple.text + "</i>");
		}
	}
}
