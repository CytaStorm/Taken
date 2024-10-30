using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueNode
{

    public string NodeName { get; set; }
    public string Info { get; set; }
    public List<DialogueNode> Links { get; set; }
    public List<string> RequiredChoices { get; private set; }
    

    public DialogueNode(string nodeName, string info, List<string> requiredChoices)
    {
        NodeName = nodeName;
        Info = info;
        RequiredChoices = requiredChoices;

        // Can't be a parameter because parser needs to run twice to get references for
        // the node links
        Links = new List<DialogueNode>(); 
    }

    public override string ToString()
    {
        return $"Node: {NodeName}\nInfo: {Info}\nLinks: {string.Join(", ", Links)}\n";
    }
}

