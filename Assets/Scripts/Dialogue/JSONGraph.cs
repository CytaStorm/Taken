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
	public List<DialogueGraph> CreateGraphs() 
	{
		List<DialogueGraph> dialogueGraphs = new List<DialogueGraph>();
		//create links
		for (int i = 0; i < passages.Count; i++)
		{
			JSONPassage passage = passages[i];
			if (passage.tags.Contains("start"))
			{
				dialogueGraphs.Add(new DialogueGraph(passage.name));

				//Collect Graph nodes
				DialogueGraph latestGraph = 
					dialogueGraphs[dialogueGraphs.Count - 1];

				//Add start node
				latestGraph.Nodes.Add(new DialogueNode(passage.name, passage.text, passage.links, passage.tags));
				latestGraph.Name = latestGraph.Nodes[0].Name;

				//Remove start node from JSON passage list
				passages.Remove(passage);
				i--;
			}
		}

		//actually link nodes
		foreach (DialogueGraph graph in dialogueGraphs)
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
	private void AddAllLinkedPassages(DialogueGraph graph)
	{
		Queue<DialogueNode> nodeQueue = new Queue<DialogueNode>();
		nodeQueue.Enqueue(graph.Nodes[0]);
		while (nodeQueue.Count > 0)
		{
			//Find and create linked nodes
			DialogueNode currentNode = nodeQueue.Dequeue();

			foreach (DialogueLink link in currentNode.Links)
			{
				//Find passage with matching name, add it to queue, remove it from JSON passage list
				JSONPassage nextPassage = passages.FirstOrDefault(passage => passage.name == link.Link);
				if (nextPassage == null) continue;

				//Found passage
				graph.Nodes.Add(new DialogueNode(nextPassage.name, nextPassage.text, nextPassage.links, nextPassage.tags));
				nodeQueue.Enqueue(graph.Nodes.Last());
				passages.Remove(nextPassage);
			}
		}

		//Make connections between nodes
		foreach (DialogueNode node in graph.Nodes)
		{
			foreach (DialogueLink link in node.Links)
			{
				link.ConnectedNode = 
					graph.Nodes.FirstOrDefault(
						connection => connection.Name == link.Link);
			}
		}
	}

}
