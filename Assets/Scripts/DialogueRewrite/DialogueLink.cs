using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DialogueLink
{
	public Dictionary<string, bool> ConditionsToCheck;

	public DialogueLink(Dictionary<string, bool> conditionsToCheck)
	{
		ConditionsToCheck = conditionsToCheck;
	}
	
	/// <summary>
	/// Compares supplied condition list to Conditions to check.
	/// </summary>
	/// <param name="conditionsList">Conditions to use to check against internal list.</param>
	/// <returns>If all conditions are satisfied.</returns>
	public bool isTrue (List<DialogueCondition> conditionsList)
	{
		foreach (DialogueCondition condition in conditionsList)
		{
			//Specified condition is not being checked
			if (!ConditionsToCheck.ContainsKey(condition.Name)) continue;

			//If there is any mismatch, then early return false.
			if (ConditionsToCheck[condition.Name] != condition.IsTrue)
			{
				return false;
			}
		}
		return true;
	}
}
