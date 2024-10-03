using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChestController : Interactable
{
	private List<string> _inventory = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    private void Update()
    {
        // Checks if NPC is focused by the player
        if (isFocus && canInteract)
        {
            float distance = Vector3.Distance(
                PlayerScript.Player.PlayerController.gameObject.transform.position,
                interactionTransform.position);

            // If its able to be interacted with, Interact
            if (distance <= radius && !hasInteracted && !isMoving)
            {
                Debug.Log("INTERACT");
                UIController.UI.OpenDialogue(); // Chest uses dialogue interface

                Interact();

                // Disables further interactions once chest is open
                hasInteracted = true;                
                canInteract = false;
                Debug.Log("Interaction disabled after initial interaction.");
                
            }
            else if (distance > radius && !isMoving)
            {
                hasInteracted = false;
            }
        }
    }

    public override void Interact()
    {
		//Open Inventory UI

        //GiveItem();
        //base.Interact();
    }

    /// <summary>
    /// If the chest hasn't been emptied, give the item to the player
    /// </summary>
    private void GiveItem()
    {
        if (!hasInteracted)
        {
            Debug.Log("Gave item to player.");
            // add code here to put item in player inventory
        }
        else
        {
            Debug.Log("Cannot give item. Chest is empty.");
        }
    }
}
