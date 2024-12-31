using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class SceneThreeController : MonoBehaviour
{
    // Actors
    public GameObject sallos;
    public GameObject eulyss;
    public GameObject akif;

    public SceneTwoManager sceneTwoManager;

    // Event trigger variables
    public List<string> flagNames;
    private List<DialogueFlag> eventFlags;
    private double timer;

    // Actor components
    private NavMeshAgent sallosAgent;
    private Animator sallosAnimator;
    private NavMeshAgent eulyssAgent;
    private Animator eulyssAnimator;
    private NavMeshAgent akifAgent;
    private Animator akifAnimator;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize timer
        timer = 0f;

        // Create eventFlags list based on string list flagNames
        eventFlags = new List<DialogueFlag>();
        foreach(string name in flagNames)
        {
            foreach (DialogueFlag flag in sceneTwoManager.dialogueFlags) 
            { 
                if (flag.Name == name)
                {
                    eventFlags.Add(flag);
                }
            }
        }

        // Get component data for all actors
        sallosAgent = sallos.GetComponent<NavMeshAgent>();
        sallosAnimator = sallos.GetComponent<Animator>();

        eulyssAgent = eulyss.GetComponent<NavMeshAgent>();
        eulyssAnimator = eulyss.GetComponent<Animator>();

        akifAgent = akif.GetComponent<NavMeshAgent>();
        akifAnimator = akif.GetComponent<Animator>();

        // Set initial positions of all actors
        sallos.transform.position = new Vector3(-1.45f, 0, 3.92f);
        eulyss.transform.position = new Vector3(-0.81f, 0, 4.82f);
        akif.transform.position = new Vector3(-2.078f, 0, -7.49f);
    }

    // Update is called once per frame
    void Update()
    {
        // Update timer each frame
        timer += Time.deltaTime;

        // Play event when the specified flag is true
        if (eventFlags[0].IsTrue)
        {
            PlayEventZero();
        }
    }

    private void PlayEventZero()
    {
        akifAgent.destination = new Vector3(-2.078f, 0, 0);
        akifAnimator.SetBool("Walking", true);

        if (akif.transform.position == akifAgent.destination)
        {
            eventFlags[0].IsTrue = false;
            akifAnimator.SetBool("Walking", false);
        }        
    }
}
