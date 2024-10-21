using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueNode
{
    public string Name { get; set; }

	public string NodeText { get; set; }

	//Rewrite
	/// <summary>
	/// List of tuples that contain the connecting node as well as the link to it.
	/// </summary>
	public List<(DialogueNode nextNode, DialogueLink link)> Links;

    public DialogueNode(string nodeName, string nodeText)
    {
        Name = nodeName;
		NodeText = nodeText;
    }
}

