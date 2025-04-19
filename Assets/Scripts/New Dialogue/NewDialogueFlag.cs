using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Custom data structure for representing condition checks in dialogue trees 
/// </summary>
public class NewDialogueFlag
{
	private bool _isTrue;
	public List<string> Names = new List<string>();

	public bool IsTrue
	{
		get { return _isTrue; }
		set
		{
			_isTrue = value;
			if (onValueChange != null)
			{
				onValueChange();
			}
		}
	}

	/// <summary>
	/// Delegate for onChange methods
	/// </summary>
	public delegate void onValueChangeHandler();

	/// <summary>
	/// Methods to call when the value changes
	/// </summary>
	public event onValueChangeHandler onValueChange;

	/// <summary>
	/// Creates a copy of a DialogueFlag, set to false.
	/// </summary>
	/// <param name="other"></param>
	public NewDialogueFlag(NewDialogueFlag other)
	{
		IsTrue = false;
		Names.AddRange(other.Names);
	}

	/// <summary>
	/// Creates a new DialogueFlag with a single flag that is set to false
	/// </summary>
	/// <param name="conditionName">Name of condition</param>
	public NewDialogueFlag(string conditionName)
	{
		IsTrue = false;
		Names.Add(conditionName);
	}

	/// <summary>
	/// Creates a DialogueFlag with an OR (Any flag in the list can be true), set to false.
	/// </summary>
	/// <param name="conditionNames">Names of conditions</param>
	public NewDialogueFlag(List<string> conditionNames)
	{
		Names = conditionNames;
	}

	/// <summary>
	/// Creates a new DialogueFlag
	/// </summary>
	/// <param name="conditionName">Name of condition</param>
	/// <param name="isTrue">True or false.</param>
	public NewDialogueFlag(string conditionName, bool isTrue)
	{
		IsTrue = isTrue;
		Names.Add(conditionName);
	}

	/// <summary>
	/// Creates a new AND DialogueFlag
	/// </summary>
	/// <param name="conditionNames">List of AND conditions</param>
	/// <param name="isTrue">True or False.</param>
	public NewDialogueFlag(List<string> conditionNames, bool isTrue)
	{
		IsTrue = isTrue;
		Names = conditionNames;
	}

	public override string ToString()
	{
		string result = string.Empty;
		foreach (string conditionName in Names)
		{
			result += $"{conditionName}, ";
		}
		result = result.Substring(0, result.Length - 2);
		return $"{result}, {IsTrue}";
	}

	public override bool Equals(object obj)
	{
		if (obj is NewDialogueFlag)
		{
			NewDialogueFlag other = (NewDialogueFlag)obj;
			return other.IsTrue == IsTrue && other.Names.SequenceEqual(Names);
		}

		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(_isTrue, Names, IsTrue);
	}
}
