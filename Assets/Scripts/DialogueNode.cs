using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueNode
{
    public string NodeName { get; set; }
    public string Info { get; set; }
    public List<string> Links { get; set; }

    public DialogueNode(string nodeName, string info)
    {
        NodeName = nodeName;
        Info = info;
        Links = new List<string>();
    }

    public override string ToString()
    {
        return $"Node: {NodeName}\nInfo: {Info}\nLinks: {string.Join(", ", Links)}\n";
    }
}

