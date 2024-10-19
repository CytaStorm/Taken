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
	[SerializeField] private GameObject _dialogueChoicesUI;
	[SerializeField] private GameObject _gameplayUI;
	[Space]
	[SerializeField] private GameObject _dialogueChoicePrefab;
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

	//public void OpenDialogue(List<string> inventory)
	//{
	//	CurrentUIMode = UIMode.Dialogue;

	//	//Draw Dialog box with inventory options

	//	//Foreach item in inventory
	//	//Create list 

	//}

	public void OpenDialogue(int choices)
	{
		CurrentUIMode = UIMode.Dialogue;
		
		//Spawn placeholder dialogue choices
		for (int i = 0; i < choices; i++)
		{
			//Spawn Dialogue choice
			Instantiate(_dialogueChoicePrefab, _dialogueChoicesUI.transform);
		}

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
