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

	/// <summary>
	/// Checks if the names of 2 flags are the same.
	/// </summary>
	/// <param name="flag1">First flag</param>
	/// <param name="flag2">Second flag</param>
	/// <returns>If the names of 2 flags are the same.</returns>
	public bool MatchName(DialogueFlag flag2)
	{
		return Name == flag2.Name ;
	}

	public static bool operator == (DialogueFlag flag1, DialogueFlag flag2)
	{
		return flag1.MatchName(flag2) && flag1.IsTrue == flag2.IsTrue;
	}

	public override bool Equals(object obj)
	{
		if (obj is DialogueFlag)
		{
			return this == obj as DialogueFlag;
		}

		return false;
	}
	public override int GetHashCode()
	{
		return GetHashCode();
	}

	public static bool operator != (DialogueFlag flag1, DialogueFlag flag2)
	{
		return !flag1.MatchName(flag2) || flag1.IsTrue != flag2.IsTrue ;
	}
}
