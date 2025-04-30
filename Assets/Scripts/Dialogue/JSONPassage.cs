using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JSONPassage
{
    public string text;
    public string name;
    public List<JSONLinks> links;
    public List<string> tags;
}
