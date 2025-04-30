using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JSONLinks
{
    /// <summary>
    /// Name to display over link, this shows up on dialogue button.
    /// </summary>
    public string name;

    /// <summary>
    /// Name of node this link connects to.
    /// </summary>
    public string link;
}
