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
    protected bool hasInteracted = false;
    public bool isFocus { get { return PlayerController.PlayerControl.Focus == this; } }

    public bool hasLimitedInteractions = true;
    protected bool canInteract = true;    
    public int interactionCount = 0;

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
		hasInteracted = true;
		interactionCount++;
		UpdateSceneGraph.Invoke(this);
    }

    protected virtual void Update()
	{
        if (isHighlighted && !isFocus)
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
        if (!isFocus || !canInteract)
		{
			return;
		}

		float distance = Vector3.Distance(
			PlayerController.PlayerControl.gameObject.transform.position,
			InteractionPoint.transform.position);

		// If its able to be interacted with, Interact
		if (distance < 0.1 && isFocus && UIManager.UI.CurrentUIMode == UIMode.Gameplay)
		{
			if (hasLimitedInteractions)
			{
				if (interactionCount > 0) { return; }
				else {interactionCount++; }
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
