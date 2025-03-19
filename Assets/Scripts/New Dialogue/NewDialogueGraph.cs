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

    public NewDialogueGraph(string name)
    {
        Name = name;
    }

    public void ParseNodes()
    {
        foreach (NewDialogueNode node in Nodes)
        {
            TwineParser.ParseNode(node);
        }
    }
}
