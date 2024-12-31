using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
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

	//scrollbar
	[SerializeField] private Scrollbar _scrollbar;

    //Dialogue Choice Buttons
    public List<GameObject> _buttons;

	[Header("Aesthetics")]
	//Used to change the prev text color
	[SerializeField] private Color _prevTextColor;
	private int endingPrevColorTagIndex = 0;
	
	private void Update()
	{
		//Debug.Log(CurrentUIMode);
	}

	void Start()
	{
        if (_sceneManager.autoImplementDialogue)
        {
            CurrentUIMode = UIMode.Dialogue;
            _dialogueUI.SetActive(true);
        }
		else
		{
            CurrentUIMode = UIMode.Gameplay;
            _dialogueUI.SetActive(false);
        }
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

		//Add title of node unless it is the first node
		//because title of node is the player's response
		if (dialogueNode.NodeName != "Intro")
		{
			_textDisplay.text += "<b><font=SpeakerFont>YOU</font></b>: ";
			_textDisplay.text += dialogueNode.NodeName + "\n\n";
		}

		//change where previous text color should end
		string prevColorTag = $"<color=#{_prevTextColor.ToHexString()}>";
		string prevColorEndTag = "</color>";

		//Add on the color tags if necessary
		//Only add color tags if is not first node
		if (_textDisplay.text.Length != 0)
		{
			//If there is no prev colored text (usually second node)
			//Color existing text prev text color
			if (_textDisplay.text.Substring(
				0, prevColorTag.Length) != prevColorTag)
			{
				_textDisplay.text = prevColorTag + 
									_textDisplay.text + 
									prevColorEndTag;
			
			} 
			else
			{
				//Remove old ending tag
				_textDisplay.text = _textDisplay.text.Remove(
					endingPrevColorTagIndex, prevColorEndTag.Length + 1);

				//replace ending tag at end of string
				_textDisplay.text += prevColorEndTag;

			}

			//Update prev color end tag position
			endingPrevColorTagIndex = _textDisplay.text.Length - prevColorEndTag.Length - 1;
		}
		
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

			if (_sceneManager.CheckTraversal(linkedNode))
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
		//print("clear!");
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
		//Debug.Log("Changed to gameplay buton");
		CurrentUIMode = UIMode.Gameplay;
		_dialogueUI.SetActive(false);
		_gameplayUI.SetActive(true);
	}
}
