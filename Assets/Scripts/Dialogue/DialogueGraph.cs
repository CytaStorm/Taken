using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DialogueGraph
{
    public string Name;
    
    public List<JSONPassage> passages;
    public List<DialogueNode> Nodes = new List<DialogueNode>();

    public DialogueNode StartNode;

    public bool traversed;

    public DialogueGraph(string name)
    {
        Name = name;
    }

    public void ParseNodes()
    {
        StartNode = Nodes[0];
        foreach (DialogueNode node in Nodes)
        {
            TwineParser.ParseNode(node);
        }
    }

    public void ReassignStart()
    {
        DialogueNode newStart;

        // If there are no links, don't reassign start
        if (StartNode.Links.Count == 0 || StartNode.Name == "newIntro") { return; }

        // find the next viable start node by acessing the first start's link
        newStart = StartNode.Links[0].ConnectedNode;

        // reassign start
        Nodes.Remove(StartNode);
        newStart.Name = "newIntro";
        StartNode = newStart;
    }
}
