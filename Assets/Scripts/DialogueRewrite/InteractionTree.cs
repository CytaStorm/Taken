using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionTree
{
	public List<DialogueNode> Nodes;

	public DialogueNode CurrentNode;

	public InteractionTree(string treeName)
	{
		AddNode(treeName + " base", "//base");
		CurrentNode = Nodes[0];
	}

	/// <summary>
	/// Adds a node to the InteractionTree.
	/// </summary>
	/// <param name="nodeName">Name of the node.</param>
	/// <exception cref="ArgumentException"></exception>
	public void AddNode(string nodeName, string nodeText)
	{
		//Check if node already exists.
		if (FindNode(nodeName) != null) throw new ArgumentException("Node with that name already exists!");

		Nodes.Add(new DialogueNode(nodeName, nodeText));
	}

	/// <summary>
	/// Adds an already existing node to the InteractionTree.
	/// </summary>
	/// <param name="node">Node to add.</param>
	/// <exception cref="ArgumentException"></exception>
	public void AddNode(DialogueNode node)
	{
		if (FindNode(node.Name) != null) throw new ArgumentException("Node with that name already exists!");
		Nodes.Add(node);
	}

	/// <summary>
	/// Forced tree traversal, guaranteed to move if index is valid.
	/// </summary>
	/// <param name="choiceIndex">Index of choice</param>
	/// <exception cref="System.IndexOutOfRangeException"></exception>
	private void TraverseTree(int choiceIndex)
	{
		if (choiceIndex > CurrentNode.Links.Count - 1)
		{
			throw new System.IndexOutOfRangeException(
				"Trying to traverse to a node that doesn't exist!");
		}	
		CurrentNode = CurrentNode.Links[choiceIndex].nextNode;
	}

	/// <summary>
	/// Traverse the InteractionTree.
	/// </summary>
	/// <param name="choiceIndex">Index of choice.</param>
	/// <param name="conditionsToCheck">Conditions to check</param>
	/// <returns>If the move was successful.</returns>
	/// <exception cref="System.IndexOutOfRangeException"></exception>
	public bool TraverseTree(int choiceIndex, List<DialogueCondition> conditionsToCheck)
	{
		if (choiceIndex > CurrentNode.Links.Count - 1)
		{
			throw new System.IndexOutOfRangeException(
				"Trying to traverse to a node that doesn't exist!");
		}
		
		if (CurrentNode.Links[choiceIndex].link.isTrue(conditionsToCheck)) 
		{
			TraverseTree(choiceIndex);
			return true;
		}

		return false;
	}

	/// <summary>
	/// Creates a link between two nodes.
	/// </summary>
	/// <param name="linkFrom">Node to link from.</param>
	/// <param name="linkTo">Node to link to.</param>
	/// <param name="link">Link to link them with.</param>
	public void AddLink(DialogueNode linkFrom, DialogueNode linkTo, DialogueLink link)
	{
		linkFrom.Links.Add((linkTo, link));
	}

	/// <summary>
	/// Finds the first Node with a matching name in the list of nodes.
	/// </summary>
	/// <param name="nodeName">Name of node to look for.</param>
	/// <returns>The matching node, or null if it doesn't exist.</returns>
	public DialogueNode FindNode(string nodeName)
	{
		DialogueNode result = Nodes.Find(x => x.Name == nodeName);
		if (result != null) return result;

		return null;
	}
}
