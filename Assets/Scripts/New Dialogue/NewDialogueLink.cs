using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDialogueLink
{
    /// <summary>
    /// Flags that must be fulfilled to accept link
    /// </summary>
    public List<NewDialogueFlag> Flags = null;

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
    public NewDialogueNode ConnectedNode = null;

    /// <summary>
    /// Temporary link object used in first pass through of graph creation
    /// </summary>
    /// <param name="_name">Name to display on dialogue button.</param>
    /// <param name="_link">Name of node it connects to.</param>
    public NewDialogueLink(string _name, string _link)
    {
        Name = _name;
        Link = _link;
    }

    public NewDialogueLink(List<NewDialogueFlag> _flags, string _name, NewDialogueNode connectedNode)
    {
        Name = _name;
        Flags = _flags;
        ConnectedNode = connectedNode;
    }
}
