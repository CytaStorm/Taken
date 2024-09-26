using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DIALOGUE NODE will only store a single dialogue option.
public class DialogueNode : MonoBehaviour
{
    //FIELDS
    private string data;
    private string buttonName;

    //PROPERTIES
    public string Data
    {
        get { return data; }
    }

    public string ButtonName
    {
        get { return buttonName; }
    }

    //CONSTRUCTOR
    public DialogueNode(string data, string buttonName)
    {
        this.data = data;
        this.buttonName = buttonName;
    }
}
