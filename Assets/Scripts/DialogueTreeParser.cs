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

        // Parses special text in each node
        foreach (DialogueNode node in dialogueNodes)
        {
            // Look through info of each node

            int startIndex = 


            string adjNames = node.Info.Substring();

            // Remove portions in square brackets

            // Delete unnecessary portions of node info
        }

        return dialogueNodes;
    }
}
