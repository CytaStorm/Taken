using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueNode
{

    public string NodeName { get; set; }
    public string Info { get; set; }
    public List<DialogueNode> Links { get; set; }
	//Debug, might remove later
    public List<string> LinkNames { get; set; }
    public List<DialogueFlag> Flags { get; set; }
    

    public DialogueNode(string nodeName, string info)
    {
        NodeName = nodeName;
        Info = info;

        // Can't be a parameter because parser needs to run twice to get references for
        // the node links
        Links = new List<DialogueNode>(); 
        Flags = new List<DialogueFlag>(); 
    }

    public override string ToString()
    {
        return $"Node: {NodeName}\nInfo: {Info}\nLinks: {string.Join(", ", Links)}\n";
    }
}

