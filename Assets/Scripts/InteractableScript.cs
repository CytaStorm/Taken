using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;  // Import the AI Navigation namespace

public class InteractableScript : MonoBehaviour
{
    [Header("References to other Gameobjects")]
    public NewDialogueGraph Graph;

    public UnityEvent<InteractableScript> UpdateSceneGraph { get; private set; }

    [Header("Interactivity")]
    public bool Interactable = true;    
    public bool HasLimitedInteractions = false;
    public int InteractionCount = 0;
    public bool isFocus { 
        get { 
            return PlayerController.PlayerControl.Focus == this; 
        } 
    }

    public GameObject InteractionPoint;
    public GameObject InteractionPivot;

	[Header("Materials")]
	[SerializeField] protected List<Material> _materialList;
    protected bool isHighlighted = false;
    protected float highlightTimer = 0f;

    protected virtual void Awake()
    {
		//print(twineFile);
        //Graph = new DialogueGraph(twineFile);
        UpdateSceneGraph = new UnityEvent<InteractableScript>();
        //Debug.Log(gameObject.name + " " + UpdateSceneGraph);
    }

    public virtual void Interact()
    {
		UIManager.UI.ChangeToDialogue();
		InteractionCount++;
		UpdateSceneGraph.Invoke(this);
    }

    protected virtual void Update()
	{
        if (isHighlighted && !isFocus && Interactable)
        {
			foreach (Material mat in _materialList)
			{
                mat.SetColor("_Tint", new Color(
                    Mathf.Abs(Mathf.Sin(highlightTimer)),
                    1,
                    1));
			}
        }
        highlightTimer += Time.deltaTime;

        //Interaction filter
        if (!isFocus || !Interactable)
		{
			return;
		}

		float distance = Vector3.Distance(
			PlayerController.PlayerControl.gameObject.transform.position,
			InteractionPoint.transform.position);

		// If its able to be interacted with, Interact
		if (distance < 0.2 && isFocus && UIManager.UI.CurrentUIMode == UIMode.Gameplay)
		{
			if (HasLimitedInteractions)
			{
				if (InteractionCount > 0) { return; }
				else {InteractionCount++; }
			}
            //hasInteracted = true;
            Interact();
		}
    }

    private void OnMouseEnter()
    {
		//Debug.Log("mouse entered");

        highlightTimer = 0f;
        isHighlighted = true;
    }

    private void OnMouseExit()
    {
        //Debug.Log("mouse exited");
		foreach (Material mat in _materialList)
		{
			mat.SetColor("_Tint", new Color(1, 1, 1));
		}
        isHighlighted = false;
    }

    //private void OnDrawGizmosSelected()
    //{
    //	Gizmos.color = Color.yellow;
    //	Gizmos.DrawWireSphere(interactionTransform.position, radius);
    //}
}
