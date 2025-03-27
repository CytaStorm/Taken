using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Rendering;
[Serializable]
public class JSONGraph
{
	public List<JSONPassage> passages;

	public string name;
	public List<NewDialogueGraph> CreateGraphs() 
	{
		List<NewDialogueGraph> dialogueGraphs = new List<NewDialogueGraph>();
		//create links
		for (int i = 0; i < passages.Count; i++)
		{
			JSONPassage passage = passages[i];
			if (passage.tags.Contains("start"))
			{
				dialogueGraphs.Add(new NewDialogueGraph(passage.name));

				//Collect Graph nodes
				NewDialogueGraph latestGraph = 
					dialogueGraphs[dialogueGraphs.Count - 1];

				//Add start node
				latestGraph.Nodes.Add(new NewDialogueNode(passage.name, passage.text, passage.links, passage.tags));
				latestGraph.Name = latestGraph.Nodes[0].Name;

				//Remove start node from JSON passage list
				passages.Remove(passage);
				i--;
			}
		}

		//actually link nodes
		foreach (NewDialogueGraph graph in dialogueGraphs)
		{
			AddAllLinkedPassages(graph);

			//Parse Dialogue nodes
			graph.ParseNodes();
		}


		return dialogueGraphs;
	}

	/// <summary>
	/// Adds all linked nodes to the graph.
	/// </summary>
	/// <param name="graph">Graph to add linked nodes to.</param>
	private void AddAllLinkedPassages(NewDialogueGraph graph)
	{
		Queue<NewDialogueNode> nodeQueue = new Queue<NewDialogueNode>();
		nodeQueue.Enqueue(graph.Nodes[0]);
		while (nodeQueue.Count > 0)
		{
			//Find and create linked nodes
			NewDialogueNode currentNode = nodeQueue.Dequeue();

			foreach (NewDialogueLink link in currentNode.Links)
			{
				//Find passage with matching name, add it to queue, remove it from JSON passage list
				JSONPassage nextPassage = passages.FirstOrDefault(passage => passage.name == link.Link);
				if (nextPassage == null) continue;

				//Found passage
				graph.Nodes.Add(new NewDialogueNode(nextPassage.name, nextPassage.text, nextPassage.links, nextPassage.tags));
				nodeQueue.Enqueue(graph.Nodes.Last());
				passages.Remove(nextPassage);
			}
		}

		//Make connections between nodes
		foreach (NewDialogueNode node in graph.Nodes)
		{
			foreach (NewDialogueLink link in node.Links)
			{
				link.ConnectedNode = 
					graph.Nodes.FirstOrDefault(
						connection => connection.Name == link.Link);
			}
		}
	}

}
