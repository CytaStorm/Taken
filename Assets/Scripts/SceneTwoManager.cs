using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTwoManager : MonoBehaviour
{
    [SerializeField] public List<DialogueFlag> dialogueChecks;
    [SerializeField] public List<GameObject> NPCs;
    public DialogueTraverser traverser;
    private DialogueGraph currentGraph;

    // Start is called before the first frame update
    void Start()
    {
        traverser = new DialogueTraverser();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
