using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
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

	public void OpenDialogue(List<string> inventory)
	{
		CurrentUIMode = UIMode.Dialogue;

		//Draw Dialog box with inventory options
	}

	public void OpenDialogue()
	{
		CurrentUIMode = UIMode.Dialogue;

		//Draw Dialogue box
		_dialogueUI.SetActive(true);
		_gameplayUI.SetActive(false);

		PlayerScript.Player.PlayerController.StopMoving();
	}

	public void ChangeToGameplay()
	{
		CurrentUIMode = UIMode.Gameplay;
		//Hide dialogue UI, show gameplay UI
		_dialogueUI.SetActive(false);
		_gameplayUI.SetActive(true);

		PlayerScript.Player.PlayerController.StartMoving();
	}
}
