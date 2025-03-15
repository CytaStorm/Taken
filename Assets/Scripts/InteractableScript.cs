using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;  // Import the AI Navigation namespace

public class InteractableScript : MonoBehaviour
{
    [Header("References to other Gameobjects")]
    public UIManager _UIManager;

    public DialogueGraph Graph { get; protected set; }
    public UnityEvent<InteractableScript> UpdateSceneGraph { get; private set; }

    [Header("Interactivity")]
    public float radius = 3f;
    protected bool hasInteracted = false;
    protected bool isFocus = false;       

    public bool hasLimitedInteractions = true;
    protected bool canInteract = true;    
    protected int interactionCount = 0;

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

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public virtual void Interact()
    {
		_UIManager.ChangeToDialogue();
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
			gameObject.transform.position);

		// If its able to be interacted with, Interact
		if (distance <= radius && !hasInteracted)
		{
			if (hasLimitedInteractions)
			{
				if (interactionCount > 0) { return; }
				else {interactionCount++; }
			}
            Interact();
		}
		else if (distance > radius)
		{
			hasInteracted = false;
		}
    }

	public void OnDefocused()
	{
		isFocus = false;
		hasInteracted = false;
	}

	public void OnFocused()
	{
		isFocus = true;
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
