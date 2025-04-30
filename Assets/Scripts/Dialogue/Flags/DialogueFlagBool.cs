using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class DialogueFlagBool : DialogueFlag
{
	/// <summary>
	/// Methods to call when the value changes
	/// </summary>
	public override event EventHandler OnValueChange;

	//Bool flag
	private bool _isTrue;

	public bool IsTrue
	{
		get { return _isTrue; }
		set
		{
			if (value != IsTrue)
			{
				_isTrue = value;
				OnValueChange?.Invoke(this, EventArgs.Empty);
            }
		}
	}

    /// <summary>
    /// Creates a copy of a DialogueFlag, set to false.
    /// </summary>
    /// <param name="other"></param>
    public DialogueFlagBool(DialogueFlagBool other)
	{
		IsTrue = false;
		Name = other.Name;
	}

	/// <summary>
	/// Creates a new DialogueFlag with a single flag that is set to false
	/// </summary>
	/// <param name="conditionName">Name of condition</param>
	public DialogueFlagBool(string name)
	{
		IsTrue = false;
		Name = name;
	}

	/// <summary>
	/// Creates a new DialogueFlag
	/// </summary>
	/// <param name="conditionName">Name of condition</param>
	/// <param name="isTrue">True or false.</param>
	public DialogueFlagBool(string name, bool isTrue)
	{
		IsTrue = isTrue;
		Name = name;
	}

	public override string ToString()
	{
		string result = Name;
		result = result.Substring(0, result.Length - 2);
		return $"{result}, {IsTrue}";
	}

	public override bool Equals(DialogueFlag obj)
    {
        if (obj is not DialogueFlagBool)
        {
            return false;
        }

        DialogueFlagBool other = (DialogueFlagBool)obj;
        return other.IsTrue == IsTrue && other.Name == Name;
    }

    public override int GetHashCode()
	{
		return HashCode.Combine(_isTrue, Name, IsTrue);
	}


}
