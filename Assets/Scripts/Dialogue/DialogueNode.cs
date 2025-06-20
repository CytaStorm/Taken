using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueNode
{
    /// <summary>
    /// Name of Node
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Text inside node. Returns reference.
    /// </summary>
    public string Text { get; set; }
    /// <summary>
    /// Tag of node
    /// </summary>
    public List<string> Tags = new List<string>();

    public List<DialogueLink> Links = new List<DialogueLink>();

    /// <summary>
    /// Dialogue Flags to change.
    /// If the Flag is a bool flag, then change it to match
    /// If the Flag is a value flag, check if relativeChange is null
    /// If relativeChange is null, directly set value, change value to match
    /// If relativeChange is not null, then de/increment by relativechange
    /// </summary>
    public List<DialogueFlag> FlagsToChange = new List<DialogueFlag>();

    public DialogueNode(string _name, string _text, List<JSONLinks> links, List<string> tags)
    {
        Name = _name;
        Text = _text;

        foreach (JSONLinks link in links)
        {
            //Temporary link, must fill out with actual details on parse
            Links.Add(new DialogueLink(link.name, link.link));
        }

        foreach (string tag in tags)
        {
            Tags.Add(tag);
        }
    }

    public DialogueNode(string _name, string _text)
    {
        Name = _name;
        Text = _text;

        // Can't be a parameter because parser needs to run twice to get references for
        // the node links
        //Links = new List<DialogueNode>(); 
        //Flags = new List<DialogueFlag>();
        //FlagsToChange = new List<DialogueFlag>();
    }

    public override string ToString()
    {
        return $"Node: {Name} || Info: {Text} || Links: {PrintLinks()}";
    }

	private string PrintLinks()
    {
        string output = "";
        foreach (DialogueLink link in Links) 
        {
            output += link.Name + " -> " + link.Link + "\n";
        }
        return output ;
    }
}
