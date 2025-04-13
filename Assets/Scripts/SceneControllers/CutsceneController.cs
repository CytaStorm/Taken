using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class CutsceneController : SceneController
{
    // Event trigger variables
    protected List<string> _flagNames = new List<string>();
    protected List<NewDialogueFlag> _eventFlags = new List<NewDialogueFlag>();
    protected double _timer = 0;

	// Update is called once per frame
	new protected void Update()
    {
        base.Update();

        // Update timer each frame
        _timer += Time.deltaTime;
    }

    protected void CreateEventFlags()
    {
        foreach (string name in _flagNames)
        {
            foreach (NewDialogueFlag flag in DialogueFlags)
            {
                if (!flag.Names.Contains(name)) continue;

                _eventFlags.Add(flag);
            }
        }
    }
} 