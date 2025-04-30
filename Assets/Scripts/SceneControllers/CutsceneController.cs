using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class CutsceneController : SceneController
{
    // Names of flags that are events
    protected List<string> _flagNames = new List<string>();

    // Dialogue Flags that will raise events
    protected List<DialogueFlag> _eventFlags = new List<DialogueFlag>();
    protected double _timer = 0;

	// Update is called once per frame
	new protected void Update()
    {
        base.Update();

        // Update timer each frame
        _timer += Time.deltaTime;
    }

    /// <summary>
    /// Identifies flags with dev added flag names and adds
    /// them to the eventflags list.
    /// </summary>
    protected void CreateEventFlags()
    {
        foreach (string name in _flagNames)
        {
            foreach (DialogueFlag flag in DialogueFlags)
            {
                if (flag.Name != name) continue;

                _eventFlags.Add(flag);
            }
        }
    }
} 