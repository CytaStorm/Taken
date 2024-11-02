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

			//Filter out metadata
			string nodeName = ParseNodeName(trimmedLine);
			if (nodeName == "StoryTitle" || nodeName == "StoryData") continue;

			//Clean data of prev node
			if (currentNode != null)
			{
				prevNode = currentNode;
				prevNode.LinkNames = RemoveSpecialText(prevNode, "[[", "]]");
				prevNode.RequiredChoices = RemoveSpecialText(prevNode, "<<", ">>");
			}

			//Start new node
			currentNode = new DialogueNode(nodeName, "");
			dialogueNodes.Add(currentNode);
		}

		// Adds the last node if it exists
		if (currentNode != null)
        {
            dialogueNodes.Add(currentNode);
			currentNode.LinkNames = RemoveSpecialText(currentNode, "[[", "]]");
			currentNode.RequiredChoices = RemoveSpecialText(currentNode, "<<", ">>");
        }

		//Create the links between nodes
		foreach (DialogueNode node in dialogueNodes)
		{
			if (node.LinkNames == null)
			{
				continue;
			}

			foreach (string linkName in node.LinkNames)
			{
				//print(linkName);
				//Find node with that name
				//var foundItem = myArray.SingleOrDefault(item => item.intProperty == someValue);

				FindMatchingNode(dialogueNodes, node, linkName);
			}
		}

		return dialogueNodes;
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
				print("matching node name: " + node.Links[node.Links.Count - 1].NodeName);
				break;
			}
		}
	}

	private static List<string> RemoveSpecialText(DialogueNode node, string startDelimiter, string endDelimiter)
	{
		List<string> results = new List<string>();
		while (node.Info.IndexOf(startDelimiter) != -1)
		{
			//Find first set of delimiters
			int linkNameStartIndex = node.Info.IndexOf(startDelimiter) + 2;
			int linkNameEndIndex = node.Info.IndexOf(endDelimiter);
			int linkedNodeNameLength = linkNameEndIndex - linkNameStartIndex;

			string linkedNodeName = node.Info.Substring(linkNameStartIndex, linkedNodeNameLength);
			results.Add(linkedNodeName);

			//Chop the link out of node.info
			node.Info = node.Info.Remove(linkNameStartIndex - 2, linkedNodeName.Length + 4);
		}
		return results;
	}
}
