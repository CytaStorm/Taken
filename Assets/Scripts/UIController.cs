using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum UIMode
{
    Gameplay,
    Dialogue
}

public class UIController : MonoBehaviour
{
	[SerializeField] private GameObject _dialogueUI;
	[SerializeField] private GameObject _gameplayUI;
    public UIMode CurrentUIMode;
	public static UIController UI
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

	public void ChangeToGameplay()
	{
		Debug.Log("Changed to gameplay buton");
		CurrentUIMode = UIMode.Gameplay;
		_dialogueUI.SetActive(false);
		_gameplayUI.SetActive(true);
	}
}
