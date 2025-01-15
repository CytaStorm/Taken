using System;
using System.Collections.Generic;
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
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip audioClip;

	[SerializeField] private SceneTwoManager _sceneManager;

	//Parent gameobjects
	[SerializeField] private GameObject _dialogueUI;
	[SerializeField] private GameObject _gameplayUI;

	[Header("Dialogue Display")]
	//Textbox stack
	[SerializeField] private Transform _textArea;

	//Textboxes
	//[SerializeField] private GameObject _textBox;

	[SerializeField] private GameObject _singleTextBoxContainer;

	//Most recent textbox
	private Transform _mostRecentTextContainer;

	#region Buttons
	[Header("Buttons")]
	//Dialogue Choice Button
	[SerializeField] private GameObject _dialogueChoiceButton;

    //Dialogue Choice Button List
    public List<GameObject> _buttons;

	//Exit Button
    [SerializeField] private GameObject _exitButton;
	#endregion

	#region Aesthetics
	[Header("Aesthetics")]
	//Used to change the prev text color
	[SerializeField] private Color _prevTextColor;
	#endregion

	#region Animation Times
	[Header("Animation")]
	[SerializeField] [Range(1f, 10f)] private float _animationSpeed;
	private float _desiredHeight;
	//The most recent
	private LayoutElement _animatingLayout 
	{ 
		get
		{
			try
			{
				return _mostRecentTextContainer.GetComponent<LayoutElement>();
			}
			//These are here to catch if there are no text boxes.
			catch (MissingReferenceException e)
			{
				return null;
			}
			catch (NullReferenceException e)
			{
				return null;
			}
			
		}
	}
	#endregion

	private void Update()
	{
		//Debug.Log(CurrentUIMode);

		//Animate
		if (_animatingLayout != null && _animatingLayout.preferredHeight < _desiredHeight)
		{
			_animatingLayout.preferredHeight += _animationSpeed * Time.deltaTime * 150;
			Mathf.Clamp(_animatingLayout.preferredHeight, 0, _desiredHeight);
		}
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
		//If there is a previous textBox, change its color to gray
		if (_mostRecentTextContainer != null)
		{
			_mostRecentTextContainer.GetChild(0).
				GetComponent<TextMeshProUGUI>().color = _prevTextColor;
			_animatingLayout.preferredHeight = _desiredHeight;
		}

		//text box references
		//Container used to animate text boxes
		Transform _textBoxContainer = 
			Instantiate(_singleTextBoxContainer, _textArea.transform).transform;

		//Textbox itself
		Transform _textBox = 
			_textBoxContainer.transform.GetChild(0);

		TextMeshProUGUI _textDisplay = 
			_textBox.GetComponent<TextMeshProUGUI>();

		//Add title of node unless it is the first node
		//because title of node is the player's response
		if (dialogueNode.NodeName != "Intro" && dialogueNode.NodeName != "Continue")
		{
			_textDisplay.text += "<b><font=SpeakerFont>YOU</font></b>: ";
			_textDisplay.text += dialogueNode.NodeName + "\n\n";
		}

		//Add dialogue
		_textDisplay.text += dialogueNode.Info;

		//Get animation info
		Canvas.ForceUpdateCanvases();
		_desiredHeight = _textBox.GetComponent<RectTransform>().rect.height;

		//if is first textbox, snap to top
		if (_animatingLayout == null)
		{
			_mostRecentTextContainer = _textBoxContainer;
			_animatingLayout.preferredHeight = _desiredHeight;

		}
		_mostRecentTextContainer = _textBoxContainer;

		//Activate dialogue UI
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
                buttonComponent.onClick.AddListener(PlaySound);
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
            buttonComponent.onClick.AddListener(PlaySound);
            buttonComponent.onClick.AddListener(ClearButtons);
            buttonComponent.onClick.AddListener(ClearText);
            buttonComponent.onClick.AddListener(ChangeToGameplay);
        }
	}

	private void PlaySound()
	{
		audioSource.clip = audioClip;
		audioSource.Play();
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
		foreach (Transform child in _textArea.transform)
		{
			Destroy(child.gameObject);
		}
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
