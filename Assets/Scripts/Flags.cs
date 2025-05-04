using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flags : MonoBehaviour
{
    //Singleton
    public static Flags Instance;
    public DialogueFlagValue Strength;
    public DialogueFlagValue Magic;
    public DialogueFlagValue Intelligence;
    public DialogueFlagValue Faith;

    public List<DialogueFlag> DialogueFlags;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
        DialogueFlags = new List<DialogueFlag>();

        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        // Create flags for stat values
        Strength = new DialogueFlagValue("strength", 0);
        DialogueFlags.Add(Strength);
        Magic = new DialogueFlagValue("magic", 0);
        DialogueFlags.Add(Magic);
        Intelligence = new DialogueFlagValue("intelligence", 0);
        DialogueFlags.Add(Intelligence);
        Faith = new DialogueFlagValue("faith", 0);
        DialogueFlags.Add(Faith);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
