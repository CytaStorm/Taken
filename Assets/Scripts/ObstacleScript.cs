using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObstacleScript : Interactable
{
    protected override void Update()
    {
        // Checks if NPC is focused by the player
        if (isFocus && canInteract)
        {
            float distance = Vector3.Distance(
                PlayerController.PlayerControl.gameObject.transform.position,
                interactionTransform.position);

            // If its able to be interacted with, Interact
            if (distance <= radius && !hasInteracted && !isMoving)
            {
                Debug.Log("INTERACT");
                UIManager.UI.ChangeToDialogue(); // Chest uses dialogue interface
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
        GiveItem();
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
