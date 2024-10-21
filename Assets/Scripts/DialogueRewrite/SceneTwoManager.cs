using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTwoManager: MonoBehaviour
{
	private InteractionTree _sallosInteraction;
	private List<DialogueCondition> _conditions = new List<DialogueCondition>();
	private List<string> _conditionNamesList = new List<string>();

	private void Start()
	{
		_sallosInteraction = new InteractionTree("sallos");
		//Set up _sallos interactions
		_conditions.Add(new DialogueCondition("hasPot"));
		_conditions.Add(new DialogueCondition("talkedToSallos"));

		foreach (DialogueCondition condition in _conditions)
		{
			_conditionNamesList.Add(condition.Name);
		}
	}

	private void Update()
	{

	}

	private void SetUpSallosTree()
	{
		DialogueNode sallosTalk1 = new DialogueNode("sallosTalk1", "Did you put the pot on the campfire?");
		DialogueNode sallosTalk2 = new DialogueNode("sallosTalk2", "Put the pot on the campfire.");
		DialogueNode sallosTalk3 = new DialogueNode("sallosTalk3", "Grab the pot from the bag and put it on the campfire.");
		DialogueNode sallosTalk4 = new DialogueNode("sallosTalk4", "You already got the pot? put it on the campfire.");

		_sallosInteraction.AddNode(sallosTalk1);
		_sallosInteraction.AddNode(sallosTalk2);
		_sallosInteraction.AddNode(sallosTalk3);
		_sallosInteraction.AddNode(sallosTalk4);

		//create links
		_sallosInteraction.AddLink(_sallosInteraction.CurrentNode, sallosTalk1,
			new DialogueLink(new Dictionary<string, bool>
			{
				{"hasPot", false},
				{"talkedToSallos", true}
			}));
		_sallosInteraction.AddLink(_sallosInteraction.CurrentNode, sallosTalk2,
			new DialogueLink(new Dictionary<string, bool>
			{
				{"hasPot", true},
				{"talkedToSallos", true}
			}));
		_sallosInteraction.AddLink(_sallosInteraction.CurrentNode, sallosTalk3,
			new DialogueLink(new Dictionary<string, bool>
			{
				{"hasPot", false},
				{"talkedToSallos", false}
			}));
		_sallosInteraction.AddLink(_sallosInteraction.CurrentNode, sallosTalk4,
			new DialogueLink(new Dictionary<string, bool>
			{
				{"hasPot", true},
				{"talkedToSallos", false}
			}));
	}
}
