using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;

/// <summary>
/// Custom data structure for representing condition checks in dialogue trees 
/// </summary>
public abstract class DialogueFlag : IEquatable<DialogueFlag>
{
    public string Name { get; protected set; }
    public abstract event EventHandler OnValueChange;
    public abstract bool Equals(DialogueFlag other);
}
