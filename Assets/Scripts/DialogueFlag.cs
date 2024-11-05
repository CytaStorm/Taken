using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Custom data structure for representing condition checks in dialogue trees 
/// </summary>
public class DialogueFlag
{
    public bool IsTrue;
    public string Name;

    /// <summary>
    /// Creates a new DialogueFlag that is set to false
    /// </summary>
    /// <param name="conditionName"></param>
    public DialogueFlag(string conditionName)
    {
        IsTrue = false;
        this.Name = conditionName;
    }

	/// <summary>
	/// Creates a new DialogueFlag
	/// </summary>
	/// <param name="conditionName"></param>
	/// <param name="isTrue"></param>
	public DialogueFlag(string conditionName, bool isTrue)
    {
        this.IsTrue = isTrue;
        this.Name = conditionName;
    }

	public override string ToString()
	{
		return $"{Name} must be {IsTrue}";
	}
}
