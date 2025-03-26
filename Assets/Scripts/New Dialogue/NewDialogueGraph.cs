using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NewDialogueGraph
{
    public string Name;
    
    public List<JSONPassage> passages;
    public List<NewDialogueNode> Nodes = new List<NewDialogueNode>();

    public NewDialogueNode StartNode;

    public bool traversed;

    public NewDialogueGraph(string name)
    {
        Name = name;
    }

    public void ParseNodes()
    {
        StartNode = Nodes[0];
        foreach (NewDialogueNode node in Nodes)
        {
            NewTwineParser.ParseNode(node);
        }
    }

    public void ReassignStart()
    {
        NewDialogueNode newStart;

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
