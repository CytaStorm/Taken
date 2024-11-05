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
    public List<string> RequiredChoices { get; set; }
    

    public DialogueNode(string nodeName, string info)
    {
        NodeName = nodeName;
        Info = info;

        // Can't be a parameter because parser needs to run twice to get references for
        // the node links
        Links = new List<DialogueNode>(); 
        RequiredChoices = new List<string>(); 
    }

    public override string ToString()
    {
        return $"Node: {NodeName} || Info: {Info} || Links: {PrintLinks()}";
    }

    private string PrintLinks()
    {
        string output = "";
        foreach (DialogueNode node in Links) 
        {
            output += node.NodeName + "\n";
        }
        return output ;
    }
}

