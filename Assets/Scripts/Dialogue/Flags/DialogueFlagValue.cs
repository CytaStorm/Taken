using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;

/// <summary>
/// Used for flag checking.
/// None is used for flag setting (no comparison needed)
/// </summary>
public enum ValueRelation{
	Less,
	Equal,
	Greater,
	None
}

public class DialogueFlagValue : DialogueFlag
{
	/// <summary>
	/// Methods to call when the value changes
	/// </summary>
	public override event EventHandler OnValueChange;

	private int _value;
	/// <summary>
	/// Used for setting values directly
	/// </summary>
	public int Value { 
		get => _value;
		set { 
			_value = value;
			OnValueChange?.Invoke(this, EventArgs.Empty);
		}
	}

	/// <summary>
	/// Exclusively used for parsing (set: ... += ... ) or (set: ... -= ...)
	/// Must be null for increments
	/// </summary>
	public int? RelativeChange = null;

	/// <summary>
	/// Used for flag checking.
	/// None is used for flag setting (no comparison needed)
	/// </summary>
	public ValueRelation Relation = ValueRelation.None;

	/// <summary>
	/// Set DialogueFlagValue to specific name and value and relation. 
	/// Use this one directly for comparison value flag
	/// </summary>
	/// <param name="name">Name of Flag</param>
	/// <param name="value">Amount to set.</param>
	public DialogueFlagValue(string name, int value, ValueRelation relation)
	{
		Name = name;
		_value = value;
		Relation = relation;
		OnValueChange += UIManager.UI.UpdateStats;
	}

	/// <summary>
	/// Default DialogueFlagValue constructor,
	/// Value is default 0.
	/// </summary>
	/// <param name="name">Name of flag.</param>
	public DialogueFlagValue(string name) : this(name, 0, ValueRelation.None) 
	{ 
	}

	/// <summary>
	/// Default DialogueFlagValue constructor,
	/// Value is default 0.
	/// </summary>
	/// <param name="name">Name of flag.</param>
	/// <param name="value">Value to change to.</param>
	public DialogueFlagValue(string name, int value) : this(name, value, ValueRelation.None) 
	{
	}

	/// <summary>
	/// Relative change constructor for DialogueFlagValue.
	/// Relation will always be none since this is a change.
	/// </summary>
	public DialogueFlagValue(int? relativeChange, string name)
	{
		Name = name;
		RelativeChange = relativeChange;
		Relation = ValueRelation.None;
	}

	/// <summary>
	/// Copy constructor.
	/// </summary>
	/// <param name="other">ValueFlag to copy.</param>
	public DialogueFlagValue(DialogueFlagValue other)
	{
		Name = other.Name;
		_value = other.Value;
		Relation = other.Relation;
		RelativeChange= other.RelativeChange;
		OnValueChange = other.OnValueChange;
	}

	public override string ToString()
	{
		string result = $"{Name}, Comparison: {Relation}, Value: {Value}";
		if (RelativeChange != null)
		{
			result += $", Change by {RelativeChange}";
		}
		return result;
	}

	public override bool Equals(DialogueFlag obj)
    {
        if (obj is not DialogueFlagValue)
        {
            return false;
        }

		DialogueFlagValue other = (DialogueFlagValue)obj;
		if (Name != other.Name)
		{
			return false;
		}

		switch (Relation)
		{
			case ValueRelation.None:
			case ValueRelation.Equal:
				return Value == other.Value;
			case ValueRelation.Greater:
				return Value > other.Value;
			case ValueRelation.Less:
				return Value < other.Value;
			default:
				return false;
		}
    }

    public override int GetHashCode()
	{
		return HashCode.Combine(Value, Name);
	}

}
