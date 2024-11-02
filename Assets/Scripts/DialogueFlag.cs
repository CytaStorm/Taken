using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Custom data structure for representing condition checks in dialogue trees 
/// </summary>
public class DialogueFlag
{
    public bool isTrue;
    public string conditionName;

    /// <summary>
    /// Creates a new DialogueFlag that is set to false
    /// </summary>
    /// <param name="conditionName"></param>
    public DialogueFlag(string conditionName)
    {
        isTrue = false;
        this.conditionName = conditionName;
    }

    /// <summary>
    /// Creates a new DialogueFlag
    /// </summary>
    /// <param name="isTrue"></param>
    /// <param name="conditionName"></param>
    public DialogueFlag(bool isTrue, string conditionName)
    {
        this.isTrue = isTrue;
        this.conditionName = conditionName;
    }
}
