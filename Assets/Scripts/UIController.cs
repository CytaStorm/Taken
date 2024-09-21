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

		//Draw Dialogue box
		_dialogueUI.SetActive(true);
		_gameplayUI.SetActive(false);
	}

	public void ChangeToGameplay()
	{
		CurrentUIMode = UIMode.Gameplay;
		_dialogueUI.SetActive(false);
		_gameplayUI.SetActive(true);
	}
}
