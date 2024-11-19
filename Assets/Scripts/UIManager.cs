using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIMode
{
    Gameplay,
    Dialogue
}

public class UIManager : MonoBehaviour
{
	public UIMode CurrentUIMode;
	[SerializeField] private SceneTwoManager _sceneManager;

	//Parent gameobjects
	[SerializeField] private GameObject _dialogueUI;
	[SerializeField] private GameObject _gameplayUI;

	//Stuff we directly change
	[SerializeField] private TextMeshProUGUI _textDisplay;
	[SerializeField] private GameObject _dialogueChoiceButton;
    [SerializeField] private GameObject _exitButton;

    //Dialogue Choice Buttons
    public List<GameObject> _buttons;

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

		_dialogueUI.SetActive(true);
		CreateButtons(dialogueNode);
	}

	private void CreateButtons(DialogueNode dialogueNode)
	{
		//Check links
		for (int i = 0; i < dialogueNode.Links.Count; i++)
		{
			DialogueNode linkedNode = dialogueNode.Links[i];

			if (SceneTwoManager.Scene.CheckTraversal(linkedNode))
			{
                //create button for each link
                GameObject newestButton =
                    Instantiate(_dialogueChoiceButton, _dialogueUI.transform, true);
                _buttons.Add(newestButton);
                newestButton.GetComponent<DialogueChoiceScript>().ButtonText.text = linkedNode.NodeName;

                int choiceIndex = i;
                Button buttonComponent = newestButton.GetComponent<Button>();
                buttonComponent.onClick.AddListener(ClearButtons);
                buttonComponent.onClick.AddListener(delegate { _sceneManager.GoToNode(choiceIndex); });
            }			
        }

		//Add exit button
		if (_buttons.Count == 0) 
		{
            GameObject exitButton =
                Instantiate(_exitButton, _dialogueUI.transform, true);
            _buttons.Add(exitButton);

            Button buttonComponent = exitButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(ClearButtons);
            buttonComponent.onClick.AddListener(ClearText);
            buttonComponent.onClick.AddListener(ChangeToGameplay);
        }
	}

	private void ClearButtons() 
	{
		print("clear!");
		while (_buttons.Count > 0)
		{
			Destroy(_buttons[0]);
			_buttons.RemoveAt(0);
		}
	}

	private void ClearText()
	{
		_textDisplay.text = string.Empty;
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
