using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLink
{
    /// <summary>
    /// Flags that must be fulfilled to accept link
    /// </summary>
    public List<DialogueFlag> Flags = new List<DialogueFlag>();

    /// <summary>
    /// Name to show over link
    /// </summary>
    public string Name;

    /// <summary>
    /// Name of node it connects to.
    /// </summary>
    public string Link;

    /// <summary>
    /// Node that it connects to.
    /// </summary>
    public DialogueNode ConnectedNode = null;

    /// <summary>
    /// Temporary link object used in first pass through of graph creation
    /// </summary>
    /// <param name="_name">Name to display on dialogue button.</param>
    /// <param name="_link">Name of node it connects to.</param>
    public DialogueLink(string _name, string _link)
    {
        Name = _name;
        Link = _link;
    }

    /// <summary>
    /// Full DialogueLink constructor.
    /// </summary>
    /// <param name="_flags">All flags required for entry.</param>
    /// <param name="_name">Name to display over link.</param>
    /// <param name="connectedNode">Connected Node</param>
    public DialogueLink(List<DialogueFlag> _flags, string _name, DialogueNode connectedNode)
    {
        Name = _name;
        Flags = _flags;
        ConnectedNode = connectedNode;
    }
}
