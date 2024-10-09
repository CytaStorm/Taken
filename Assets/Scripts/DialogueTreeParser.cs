using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


class DialogueTreeParser
{
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
        DialogueNode currentNode = null;
        foreach (string line in fileLines)
        {
            // Trims whitespace from the line
            string trimmedLine = line.Trim();

            // Checks if the line starts with :: meaning its a new node
            if (trimmedLine.StartsWith("::"))
            {
                // If a node is open, add it to the node
                if (currentNode != null)
                {
                    dialogueNodes.Add(currentNode);
                }

                // Parses the node name, removes the :: and any other info in the braces
                int braceIndex = trimmedLine.IndexOf("{");
                string nodeName = (braceIndex > 0) ? trimmedLine.Substring(2, braceIndex - 2).Trim() : trimmedLine.Substring(2).Trim();

                // Start a new node 
                // Make sure node isn't the story title or story data
                if ((nodeName != "StoryTitle") && (nodeName != "StoryData"))
                {
                    currentNode = new DialogueNode(nodeName, "");
                }                
            }
            else if (trimmedLine.StartsWith("[[") && trimmedLine.EndsWith("]]"))
            {
                // DO NOTHING
            }
            else if (currentNode != null)
            {
                // Adds the info under the node
                currentNode.Info += trimmedLine + " ";
            }
        }

        // Adds the last node if it exists
        if (currentNode != null)
        {
            dialogueNodes.Add(currentNode);
        }

        // Prints out the parsed nodes for testing
        foreach (var node in dialogueNodes)
        {
            Debug.Log(node);
        }

        // SECOND TIME THROUGH -- assign references to adjacent nodes
        currentNode = null;
        foreach (string line in fileLines)
        {
            // Trims whitespace from the line
            string trimmedLine = line.Trim();

            // Checks if the line starts with :: meaning its a new node
            if (trimmedLine.StartsWith("::"))
            {
                // Parses the node name, removes the :: and any other info in the braces
                int braceIndex = trimmedLine.IndexOf("{");
                string nodeName = (braceIndex > 0) ? trimmedLine.Substring(2, braceIndex - 2).Trim() : trimmedLine.Substring(2).Trim();

                // Start a new node 
                // Make sure node isn't the story title or story data
                if ((nodeName != "StoryTitle") && (nodeName != "StoryData"))
                {
                    // Before starting a new node, we update the list of links of the node in
                    // dialogueNodes that matches currentNode

                    // Finds the node in the node list that is identical to currentNode
                    // Set this node's links to be that of currentNode
                    foreach (DialogueNode node in dialogueNodes)
                    {
                        if (node.NodeName == currentNode.NodeName)
                        {
                            node.Links = currentNode.Links;
                        }
                    }

                    currentNode = new DialogueNode(nodeName, "");
                }
            }
            else if (trimmedLine.StartsWith("[[") && trimmedLine.EndsWith("]]"))
            {
                // Gets the name of the link node from the hard brackets
                string link = trimmedLine.Substring(2, trimmedLine.Length - 4);

                // Finds the reference to the node with the same name as the link
                // Add this node to the currentNode's links
                DialogueNode linkNode = null;
                foreach (DialogueNode node in dialogueNodes)
                {
                    if (node.NodeName == link)
                    {
                        linkNode = node;
                    }
                }
                currentNode?.Links.Add(linkNode);
            }
            else if (currentNode != null)
            {
                // Adds the info under the node
                currentNode.Info += trimmedLine + " ";
            }
        }

        return dialogueNodes;
    }
}
