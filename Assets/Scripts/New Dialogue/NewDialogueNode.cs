using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class NewDialogueNode
{
    /// <summary>
    /// Name of Node
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Text inside node.
    /// </summary>
    public string Text { get; set; }
    /// <summary>
    /// Tag of node
    /// </summary>
    public List<string> Tags = new List<string>();

    public List<NewDialogueLink> Links = new List<NewDialogueLink>();

    public List<NewDialogueFlag> FlagsToChange = new List<NewDialogueFlag>();

    public delegate void OnEnterHandler();

    public event OnEnterHandler onEnter;

    public NewDialogueNode(string _name, string _text, List<JSONLinks> links, List<string> tags)
    {
        Name = _name;
        Text = _text;

        foreach (JSONLinks link in links)
        {
            //Temporary link, must fill out with actual details on parse
            Links.Add(new NewDialogueLink(link.name, link.link));
        }

        foreach (string tag in tags)
        {
            Tags.Add(tag);
        }
    }

    public NewDialogueNode(string _name, string _text)
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
        foreach (NewDialogueLink link in Links) 
        {
            output += link.Name + " -> " + link.Link + "\n";
        }
        return output ;
    }
}
