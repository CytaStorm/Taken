using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DialogueCondition
{
	public string Name;
	public bool IsTrue;

	public DialogueCondition(string name)
	{
		Name = name;
		IsTrue = false;
	}
}
