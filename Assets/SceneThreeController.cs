using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class SceneThreeController : MonoBehaviour
{
    public GameObject sallos;
    public GameObject eulyss;
    public List<DialogueFlag> eventFlags;

    private double timer;

    private NavMeshAgent sallosAgent;
    private Animator sallosAnimator;

    private NavMeshAgent eulyssAgent;
    private Animator eulyssAnimator;



    // Start is called before the first frame update
    void Start()
    {
        // Initialize timer
        timer = 0f;

        // Get component data for all actors
        sallosAgent = sallos.GetComponent<NavMeshAgent>();
        sallosAnimator = sallos.GetComponent<Animator>();

        eulyssAgent = eulyss.GetComponent<NavMeshAgent>();
        eulyssAnimator = eulyss.GetComponent<Animator>();

        // Set initial positions of all actors
        sallos.transform.position = new Vector3(-1.31f, 0f, 13.3f);

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

    void PlayEventZero()
    {
        // Execute event code here

        // Set event flag to false once event has ended
        // Event could be time based, so this if statement could involve the time reaching a value
        if (true)
        {

        }
    }
}
