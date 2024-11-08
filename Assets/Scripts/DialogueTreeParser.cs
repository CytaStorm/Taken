using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using Unity.VisualScripting;


class DialogueTreeParser : MonoBehaviour
{
	private void Start()
	{
		
	}
	public static List<DialogueNode> ParseFile(string filePath)
    {
        // Makes sure the file exists
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found at the path: " + filePath);
            return null;
        }

        // Makes a list for the dialogue nodes
        List<DialogueNode> dialogueNodes = new List<DialogueNode>();
        string[] fileLines = File.ReadAllLines(filePath);

		// FIRST TIME THROUGH -- create nodes and their data
		DialogueNode prevNode = null;
        DialogueNode currentNode = null;
		foreach (string line in fileLines)
		{
			// Trims whitespace from the line
			string trimmedLine = line.Trim();

			//Line is not a node starter line
			if (!trimmedLine.StartsWith("::"))
			{
				//If no node is being made, ignore.
				if (currentNode == null || trimmedLine == string.Empty) continue;

				// Adds the info under the node
				currentNode.Info += trimmedLine + "\n";
				continue;
			}

			//Filter out metadata/bad lines
			string nodeName = ParseNodeName(trimmedLine);
			if (nodeName == "StoryTitle" || nodeName == "StoryData") continue;


			if (currentNode != null)
			{
				ParseSpecialText(currentNode);
			}

			//Start new node
			currentNode = new DialogueNode(nodeName, "");
			dialogueNodes.Add(currentNode);
		}
		//Parse specialText in lastNode 
		ParseSpecialText(currentNode);

		//Create the links between nodes
		foreach (DialogueNode node in dialogueNodes)
		{
			print(node.Info);
			if (node.LinkNames == null) continue;

			foreach (string linkName in node.LinkNames)
			{
				FindMatchingNode(dialogueNodes, node, linkName);
			}
		}

		return dialogueNodes;
    }

	private static void ParseSpecialText(DialogueNode currentNode)
	{
		currentNode.LinkNames = RemoveSpecialText(currentNode, "[[", "]]");
		AddFlags(currentNode);
		RemoveSpecialText(currentNode, "(if:", "]\n");
		RemoveSpecialText(currentNode, "(else-if:", "]\n");
		RemoveSpecialText(currentNode, "(else:", "]\n");
		ParseChangeFlags(currentNode);
	}

	private static string ParseNodeName(string trimmedLine)
	{
		int braceIndex = trimmedLine.IndexOf("{");
		string nodeName =
			(braceIndex > -1) ? trimmedLine.Substring(2, braceIndex - 2)
			.Trim() : trimmedLine.Substring(2).Trim();
		return nodeName;
	}

	private static void FindMatchingNode(List<DialogueNode> dialogueNodes, DialogueNode node, string linkName)
	{
		foreach (DialogueNode innerNode in dialogueNodes)
		{
			if (innerNode.NodeName == linkName)
			{
				node.Links.Add(innerNode);
				//print("matching node name: " + node.Links[node.Links.Count - 1].NodeName);
				break;
			}
		}
	}

	private static List<string> RemoveSpecialText(
		DialogueNode node, string startDelimiter, string endDelimiter)
	{
		List<string> results = new List<string>();
		while (node.Info.IndexOf(startDelimiter) != -1)
		{
			//Find first set of delimiters
			int linkNameStartIndex = node.Info.IndexOf(startDelimiter) + startDelimiter.Length;
			int linkNameEndIndex = node.Info.IndexOf(endDelimiter);
			int linkedNodeNameLength = linkNameEndIndex - linkNameStartIndex;

			string linkedNodeName = node.Info.Substring(linkNameStartIndex, linkedNodeNameLength);
			results.Add(linkedNodeName);

			//Chop the link out of node.info
			node.Info = node.Info.Remove(
				linkNameStartIndex - startDelimiter.Length,
				linkedNodeName.Length + startDelimiter.Length + endDelimiter.Length);
		}
		return results;
	}

	private static void ParseChangeFlags(DialogueNode node)
	{
		//Get the string to parse
		List<string> stringToParse = RemoveSpecialText(node, "(set: ", ")\n");
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
		node.FlagsToChange.Add(new DialogueFlag(splitStringToParse[0], changeFlagTo));
	}

	private static void AddFlags(DialogueNode node)
	{
		//Trues
		foreach (string trues in RemoveSpecialText(node, "<<", ">>"))
		{
			node.Flags.Add(new DialogueFlag(trues, true));
		}

		//Falses
		foreach (string falses in RemoveSpecialText(node, "~~", "``"))
		{
			node.Flags.Add(new DialogueFlag(falses, false));
		}
	}
}
