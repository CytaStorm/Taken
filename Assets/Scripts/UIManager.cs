using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UIMode
{
    Gameplay,
    Dialogue
}

public class UIManager : MonoBehaviour
{
	public UIMode CurrentUIMode;

	public static UIManager UI { get; private set; }

	[Header("Other GameObjects")]
	[SerializeField] private SceneController _sceneController;
	[SerializeField] private PlayerInput _input;

	[Header("Audio")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip audioClip;

	//Parent gameobjects
	[Header("UI Groups")]
	[SerializeField] private GameObject _dialogueUI;
	[SerializeField] private GameObject _pauseMenu;

	[Header("Dialogue Display")]
	//Textbox stack
	[SerializeField] private Transform _textArea;

	//Textboxes
	[SerializeField] private GameObject _singleTextBoxContainer;

	//Most recent textbox
	private Transform _mostRecentTextContainer;


	#region Buttons
	[Header("Buttons")]
	//Dialogue Choice Button
	[SerializeField] private GameObject _dialogueChoiceButton;
	[SerializeField] private GameObject _buttonContainer;

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

	/// <summary>
	/// Singleton
	/// </summary>
    private void Awake()
    {
		if (UI != null && UI != this)
		{
			Destroy(UI);
		} else
		{
			UI = this;
		}
    }

    void Start()
	{
        if (_sceneController.autoStartDialogue)
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

	public void ChangeToDialogue()
	{
		CurrentUIMode = UIMode.Dialogue;

		//Start the dialogue traversing in DialogueTraverser
		//dialogueTraverser.StartDialogue();

		//Draw Dialogue box
		_dialogueUI.SetActive(true);
	}

	public void NewDialogueLink(NewDialogueLink dialogueLink)
	{
		if (dialogueLink.Name != "Continue" && dialogueLink.Name != "Leave")
		{

			Transform _textBoxContainer;
			Transform _textBox;
			TextMeshProUGUI _textDisplay;

			//History text
			_textBoxContainer =
				Instantiate(_singleTextBoxContainer, _textArea.transform).transform;
			_textBox =
				_textBoxContainer.transform.GetChild(0);
			_textDisplay =
				_textBox.GetComponent<TextMeshProUGUI>();
			_textDisplay.text += dialogueLink.Name;

			_mostRecentTextContainer.GetChild(0).
				GetComponent<TextMeshProUGUI>().color = _prevTextColor;

			//_mostRecentTextContainer = _textBoxContainer;

			//_animatingLayout.preferredHeight = _desiredHeight;

			//_desiredHeight = _textBox.GetComponent<RectTransform>().rect.height;

			Canvas.ForceUpdateCanvases();
			_desiredHeight = _textBox.GetComponent<RectTransform>().rect.height;

			//if is first textbox, snap to top
			if (_animatingLayout == null)
			{
				_mostRecentTextContainer = _textBoxContainer;
				_animatingLayout.preferredHeight = _desiredHeight;

			}
			_mostRecentTextContainer = _textBoxContainer;

		}
		NewDialogueNode(dialogueLink.ConnectedNode);
	}

	//Player traversed to a new Dialogue node
	public void NewDialogueNode(NewDialogueNode dialogueNode)
	{
		//If there is a previous textBox, change its color to gray
		if (_mostRecentTextContainer != null)
		{
			_mostRecentTextContainer.GetChild(0).
				GetComponent<TextMeshProUGUI>().color = _prevTextColor;
			_animatingLayout.preferredHeight = _desiredHeight;
		}

		Transform _textBoxContainer;
		Transform _textBox;
		TextMeshProUGUI _textDisplay;


		// New text
		//text box references
		//Container used to animate text boxes
		_textBoxContainer = 
			Instantiate(_singleTextBoxContainer, _textArea.transform).transform;

		//Textbox itself
		_textBox = 
			_textBoxContainer.transform.GetChild(0);
		_textDisplay = 
			_textBox.GetComponent<TextMeshProUGUI>();

		////Add title of node unless it is the first node
		////because title of node is the player's response
		//if (dialogueNode.NodeName != "Intro" && 
		//	dialogueNode.NodeName != "Continue" && 
		//	dialogueNode.NodeName != "newIntro" &&
		//	dialogueNode.NodeName != "Continue ->")
		//{
		//	_textDisplay.text += "<b><font=SpeakerFont>YOU</font></b>: ";
		//	_textDisplay.text += dialogueNode.NodeName + "\n\n";
		//}

		//Add dialogue
		_textDisplay.text += dialogueNode.Text;

		while (_dialogueUI.activeInHierarchy == false ||
			_dialogueUI.activeSelf == false)
		{
			_dialogueUI.SetActive(true);
		}

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

	private void CreateButtons(NewDialogueNode dialogueNode)
	{
		//Check links
		for (int i = 0; i < dialogueNode.Links.Count; i++)
		{
			NewDialogueLink link = dialogueNode.Links[i];
			NewDialogueNode linkedNode = link.ConnectedNode;

			if (_sceneController.CheckTraversal(link.Flags))
			{
				//Local variable is used here b/c delegates will capture full local context...
				//means that if i is used directly the delegates will capture i++ because
				//the enclosing for loop will add 1 to i at the end of each loop.
				int choiceIndex = i;

				//Create exit button, if it exists
				if (linkedNode.Tags != null &&
					linkedNode.Tags.Count != 0 &&
					linkedNode.Tags.Contains("exit"))
				{
					GameObject exitButton =
						Instantiate(_exitButton, _buttonContainer.transform, true);
					_buttons.Add(exitButton);
					exitButton.GetComponent<DialogueChoiceScript>().ButtonText.text = "<i>" + link.Name + "</i>";
					Button exitButtonComponent = exitButton.GetComponent<Button>();

					exitButtonComponent.onClick.AddListener(
						delegate { _sceneController.GoToNode(choiceIndex); });
					exitButtonComponent.onClick.AddListener(
						delegate
						{
							if (PlayerController.PlayerControl.Focus != null &&
							PlayerController.PlayerControl.Focus is NPCScript)
							{
								((NPCScript)PlayerController.PlayerControl.Focus).ExitDialogue();
							}
						}
					);
					exitButtonComponent.onClick.AddListener(PlaySound);
					exitButtonComponent.onClick.AddListener(ClearButtons);
					exitButtonComponent.onClick.AddListener(ClearText);
					exitButtonComponent.onClick.AddListener(ChangeToGameplay);

					continue;
				}

				//create button for each link
				GameObject newestButton =
					Instantiate(_dialogueChoiceButton, _buttonContainer.transform, true);
				_buttons.Add(newestButton);
				newestButton.GetComponent<DialogueChoiceScript>().ButtonText.text = link.Name;
				Button buttonComponent = newestButton.GetComponent<Button>();
				buttonComponent.onClick.AddListener(PlaySound);
				buttonComponent.onClick.AddListener(ClearButtons);
				buttonComponent.onClick.AddListener(
					delegate { _sceneController.GoToNode(choiceIndex); });
			}
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
		PlayerController.PlayerControl.RemoveFocus();
	}

	public void UnPause()
	{
		_input.currentActionMap = _input.actions.FindActionMap("Movement");
		Time.timeScale = 1;
		_pauseMenu.SetActive(false);
	}
    public void OnPause(InputAction.CallbackContext ctx)
	{
		if (!ctx.performed) return;

		//Pause
		_input.currentActionMap = _input.actions.FindActionMap("Menu");
		Time.timeScale = 0;
		_pauseMenu.SetActive(true);
	}

	public void ExitPause(InputAction.CallbackContext ctx)
	{
		if (!ctx.performed) return;
		//Unpause
		UnPause();
	}

	public IEnumerator PauseAllButtons(float seconds)
    {
        yield return new WaitForSeconds(0.01f);

        // disable all buttons
        Debug.Log("started");
        foreach(GameObject button in _buttons)
        {
            print(button.GetComponentInChildren<TMP_Text>().text);
            button.SetActive(false);
        }


        // wait for seconds amount of time
        yield return new WaitForSeconds(seconds);

        // enable all buttons
        Debug.Log("ended");
        foreach (GameObject button in _buttons)
        {
            button.SetActive(true);
        }
    }
}
