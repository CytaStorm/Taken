using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChestController : Interactable
{
    private bool isEmpty;

    // Start is called before the first frame update
    void Start()
    {
        isEmpty = false;
        base.Start();
    }

    private void Update()
    {
        base.Update();
    }

    public override void Interact()
    {
        GiveItem();
        base.Interact();
    }

    private void MoveToPosition(Vector3 destination)
    {
        base.MoveToPosition(destination);
    }

    public void OnDefocused()
    {
        base.OnDefocused();
    }

    public void OnFocused()
    {
        base.OnFocused();
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
