using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : Interactable
{
    private bool isEmpty;

    // Start is called before the first frame update
    void Start()
    {
        isEmpty = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        GiveItem();
        base.Interact();
    }

    /// <summary>
    /// If the chest hasn't been emptied, give the item to the player
    /// </summary>
    private void GiveItem()
    {
        if (!isEmpty)
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
