using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DIALOGUE NODE will only store a single dialogue option.
public class DialogueNode : MonoBehaviour
{
    //FIELDS
    private string data;

    //PROPERTIES
    public string Data
    {
        get { return data; }
    }

    //CONSTRUCTOR
    public DialogueNode(string data)
    {
        this.data = data;
    }
}
