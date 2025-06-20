using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Rendering;
[Serializable]
public class JSONGraph
{
	public List<JSONPassage> passages;

	public string name;

	private List<DialogueNode> _allNodes;
	public List<DialogueGraph> CreateGraphs() 
	{
		_allNodes = new List<DialogueNode>();
		List<DialogueGraph> dialogueGraphs = new List<DialogueGraph>();

		while (passages.Count != 0)
		{
			//Create nodes
			CreateAllLinkedNodes();
		}

		//Create graphs and give first start node
		foreach (DialogueNode node in _allNodes)
		{
			if (node.Tags.Contains("start"))
			{
				dialogueGraphs.Add(new DialogueGraph(node.Name));

				//Collect Graph nodes
				DialogueGraph latestGraph = 
					dialogueGraphs[dialogueGraphs.Count - 1];

				//Add start node
				latestGraph.Nodes.Add(node);
				latestGraph.Name = latestGraph.Nodes[0].Name;
			};
		}

		//Assign nodes to graphs
		foreach (DialogueGraph graph in dialogueGraphs)
		{
			AddAllLinkedPassages(graph);

			//Parse Dialogue nodes specialtext
			graph.ParseNodes();
		}

		//squash graphs down, since nodes can be part of mulitple graphs!!
		//nodes may belong to multiple graphs because that allows us to have
		//multiple enty points into one graph
		//first node that is part of two graphs, pick out other graph that it
		//is part of (lesser graph, has less nodes), then add all nodes from
		//lesser graph to greater graph, then delete lesser graph

		//linq not used here because we have to delete graphs
		//as necessary
		for (int graphIndex = 0; 
			graphIndex < dialogueGraphs.Count;
			graphIndex++)
		{
			DialogueGraph firstGraph = dialogueGraphs[graphIndex];
			for (int compareIndex = 1; 
				compareIndex < dialogueGraphs.Count;
				compareIndex++)
			{
				DialogueGraph secondGraph = dialogueGraphs[compareIndex];
				if (firstGraph == secondGraph) continue;

				//foreach (var node in firstGraph.Nodes.Intersect(secondGraph.Nodes))
				//{
				//	Debug.WriteLine(node.Name);
				//}

				IEnumerable<DialogueNode> intersect = 
					firstGraph.Nodes.Intersect(secondGraph.Nodes);

				//No overlap
				if (intersect.Count() == 0)
				{
					Debug.WriteLine($"{firstGraph.Name}, {secondGraph.Name}");
					continue;
				}

				//There is overlap between graphs
				DialogueGraph greaterGraph;
				DialogueGraph lesserGraph;
				if (firstGraph.Nodes.Count > secondGraph.Nodes.Count)
				{
					greaterGraph = firstGraph;
					lesserGraph = secondGraph;
				} else
				{
					greaterGraph = secondGraph;
					lesserGraph = firstGraph;
				}
				//Combine graphs
				greaterGraph.Nodes = 
					greaterGraph.Nodes.Union(lesserGraph.Nodes).ToList();

				//Delete lesser graph
				dialogueGraphs.Remove(lesserGraph);
			}
		}

		//create connections between nodes
		//this is linq fanciness to not have to 
		//create super nested code, I have to
		//loop thru each graph, each graph's nodes,
		//and each node's connections
		foreach ((DialogueGraph graph, DialogueLink link) in
			from DialogueGraph graph in dialogueGraphs
			from DialogueNode node in graph.Nodes
			from DialogueLink link in node.Links
			select (graph, link))
		{
			link.ConnectedNode = graph.Nodes.FirstOrDefault(
				connection => connection.Name == link.Link);
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
				//Find matching node in passage list
				DialogueNode nextNode = 
						_allNodes.FirstOrDefault(
							existingNode => existingNode.Name == link.Link);

				//completely doesn't exist [should not happen!]
				if (nextNode == null) {
					throw new NullReferenceException(
						"Node links to a passage that doesn't exist!");
				};
				graph.Nodes.Add(nextNode);
				nodeQueue.Enqueue(nextNode);
			}
		}
	}

	/// <summary>
	/// Converts all JSONPassages into DialogueNodes.
	/// </summary>
	/// <exception cref="NullReferenceException">
	/// Missing node, Twine / Twison broken.</exception>
	private void CreateAllLinkedNodes()
	{
		JSONPassage firstPassage = passages.First();
		_allNodes.Add(new DialogueNode(
			firstPassage.name,
			firstPassage.text,
			firstPassage.links,
			firstPassage.tags));
		passages.Remove(firstPassage);

		Queue<DialogueNode> nodeQueue = new Queue<DialogueNode>();
		nodeQueue.Enqueue(_allNodes[0]);

		while (nodeQueue.Count > 0)
		{
			//Find and create linked nodes
			DialogueNode currentNode = nodeQueue.Dequeue();

			foreach (DialogueLink link in currentNode.Links)
			{
				//Find matching node in passage list
				DialogueNode newestNode;
				JSONPassage nextPassage = 
					passages.FirstOrDefault(
						passage => passage.name == link.Link);
				if (nextPassage != null)
				{
					newestNode = new DialogueNode(
						nextPassage.name,
						nextPassage.text,
						nextPassage.links,
						nextPassage.tags);
				}
				//node already exists, don't need to add to
				//_allNodes
				else
				{
					newestNode = 
						_allNodes.FirstOrDefault(
							existingNode => existingNode.Name == link.Link);
					continue;
				};

				//completely doesn't exist [should not happen!]
				if (newestNode == null) {
					throw new NullReferenceException(
						"Node links to a passage that doesn't exist!");
				};

				//found Node
				_allNodes.Add(newestNode);
				nodeQueue.Enqueue(newestNode);
				
				//Remove node from passage list
				passages.Remove(nextPassage);
			}
		}
	}
}
