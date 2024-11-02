using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public string filePath;

    public DialogueGraph Graph { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        Graph = new DialogueGraph(filePath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
