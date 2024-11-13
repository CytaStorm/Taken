using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public enum UIMode
{
    Gameplay,
    Dialogue
}

public class UIManager : MonoBehaviour
{
	public UIMode CurrentUIMode;

	//Parent gameobjects
	[SerializeField] private GameObject _dialogueUI;
	[SerializeField] private GameObject _gameplayUI;

	//Stuff we directly change
	[SerializeField] private TextMeshProUGUI _textDisplay;
	[SerializeField] private GameObject _dialogueChoices;
	[SerializeField] private GameObject _dialogueChoiceButton;

	public static UIManager UI
	{
		get; private set;
	}

	private void Update()
	{
		//Debug.Log(CurrentUIMode);
	}

	private void Awake()
	{
		if (UI != null && UI != this)
		{
			Destroy(gameObject);
		}
		else
		{
			UI = this;
		}
	}

	void Start()
	{
		CurrentUIMode = UIMode.Gameplay;
		_dialogueUI.SetActive(false);

	}

	public void ChangeToDialogue()
	{
		CurrentUIMode = UIMode.Dialogue;

		//Start the dialogue traversing in DialogueTraverser
		//dialogueTraverser.StartDialogue();

		//Draw Dialogue box
		_dialogueUI.SetActive(true);
		_gameplayUI.SetActive(false);
	}

	//Player traversed to a new Dialogue node
	public void NewDialogueNode(DialogueNode dialogueNode)
	{
		//Add new node text to text display
		_textDisplay.text += dialogueNode.Info;

		_dialogueChoices.SetActive(true);
		CreateButtons(dialogueNode);
	}

	private void CreateButtons(DialogueNode dialogueNode)
	{
		//Check links
		foreach (DialogueNode linkedNode in dialogueNode.Links)
		{
			//create button for each link
			GameObject newestButton = 
				Instantiate(_dialogueChoiceButton, _dialogueChoices.transform, true);
			newestButton.GetComponent<DialogueChoiceScript>().ButtonText.text = linkedNode.NodeName;
		}	
	}

	//Create text for dialogue to display on screen
	private void CreateDialogue()
	{
	}

	public void ChangeToGameplay()
	{
		Debug.Log("Changed to gameplay buton");
		CurrentUIMode = UIMode.Gameplay;
		_dialogueUI.SetActive(false);
		_gameplayUI.SetActive(true);
	}
}
