using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.ComponentModel;

public class DialogueGraph : MonoBehaviour
{
    //FIELDS
    //Streamreader stuff
    [SerializeField] private StreamReader reader;
    [SerializeField, Description("Path after Assets/")] 
    private string localPath;
    string filePath;

    // Start is called before the first frame update
    void Start()
    {
        //For now, just load data on start
        LoadDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadDialogue()
    {
        //Define stream and streamreaders
        filePath = Application.dataPath + localPath;
        reader = new StreamReader(filePath);

        //Peek will return -1 at end of the file
        while(reader.Peek() >= 0)
        {
            //Code reading logic here:
        }
    }
}
