using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SceneThreeController : SceneController
{
    // Actors
    [SerializeField] private GameObject _sallos;
    [SerializeField] private GameObject _eulyss;
    [SerializeField] private GameObject _akif;
    [SerializeField] private GameObject _goon;
    
    // Actor components
    private NavMeshAgent _sallosAgent;
    [SerializeField] private GameObject _sallosMesh;
    private Animator _sallosAnimator;

    private NavMeshAgent _eulyssAgent;
    [SerializeField] private GameObject _playerMesh;
    private Animator _eulyssAnimator;

    private NavMeshAgent _akifAgent;
    [SerializeField] private GameObject _akifMesh;
    private Animator _akifAnimator;

    private NavMeshAgent _goonAgent;
    private Animator _goonAnimator;

    //Materials
	[Space(10)] [SerializeField] protected Material _sallosMaterial;

    // Start is called before the first frame update
    // MAKE SURE EXECUTION ORDER IS SET TO LAST FOR THIS TO WORK
    new void Start()
    {
        base.Start();

		_sallosMaterial.SetFloat("_Dissolve_Effect", 0);

        //Add flags
        _flagNames.Add("walkLeft");
        _flagNames.Add("enterAkif");
        _flagNames.Add("enterGoon");
        _flagNames.Add("walkCloser");
        _flagNames.Add("stab");
        _flagNames.Add("disappear");
        _flagNames.Add("reach");

        //Create eventFlags list based on string list flagNames
        CreateEventFlags();

        // Assign all event methods to OnClick() events for all buttons
        _eventFlags[0].OnValueChange += delegate { WalkLeft(); };
        _eventFlags[1].OnValueChange += delegate { EnterAkif(); };
        _eventFlags[2].OnValueChange += delegate { EnterGoon(); };
        _eventFlags[3].OnValueChange += delegate { WalkCloser(); };
        _eventFlags[4].OnValueChange += delegate { Stab(); };
        _eventFlags[5].OnValueChange += delegate { Disappear(); };
        _eventFlags[6].OnValueChange += delegate { Reach(); };

        // Get component data for all actors
        _sallosAgent = _sallos.GetComponent<NavMeshAgent>();
        _sallosAnimator = _sallosMesh.GetComponent<Animator>();

        _eulyssAgent = _eulyss.GetComponent<NavMeshAgent>();
        _eulyssAnimator = _playerMesh.GetComponent<Animator>();

        _akifAgent = _akif.GetComponent<NavMeshAgent>();
        _akifAnimator = _akifMesh.GetComponent<Animator>();

        _goonAgent = _goon.GetComponent<NavMeshAgent>();
        _goonAnimator = _goon.GetComponent<Animator>();

        // Set initial positions of all actors
        _sallos.transform.position = new Vector3(-0.41f, 0, 0.2f);
        _eulyss.transform.position = new Vector3(-0.93f, 0, -0.49f);
        _akif.transform.position = new Vector3(-4.92f, 0, 16.85f);
        _goon.transform.position = new Vector3(-12.08f, 0, 4.37f);
    }

    
    // Update is called once per frame
    new void Update()
    {
        base.Update(); // Update timer each frame
    }

    private void WalkLeft()
    {
        // Move sallos and eulyss along forest trail
        _sallosAgent.SetDestination(new Vector3(-2.11f, 0f, 7.35f));
        _eulyssAgent.SetDestination(new Vector3(-2.27f, 0f, 5.86f));
        StartCoroutine(UIManager.UI.PauseAllButtons(3f));
    }

    private void EnterAkif()
    {
        // Move akif towards sallos and eulyss
        _akifAgent.SetDestination(new Vector3(-2.990002f, 0f, 10.45f));
        StartCoroutine(UIManager.UI.PauseAllButtons(2.7f));
    }

    private void EnterGoon()
    {
        // Move the goon to corner sallos and eulyss
        _goonAgent.SetDestination(new Vector3(-4.56f, 0f, 7.1f));
        _sallosAnimator.SetTrigger("LookLeftTrigger");
        _eulyssAnimator.SetTrigger("LookLeftTrigger");
        StartCoroutine(ChangeLookLeft(_sallosAnimator, 1, 0.5f));
        StartCoroutine(ChangeLookLeft(_eulyssAnimator, 1, 0.66f));
        StartCoroutine(UIManager.UI.PauseAllButtons(2.9f));
    }

    private void WalkCloser()
    {
        // Move both akif and the goon closer to sallos and eulyss
        _akifAgent.SetDestination(new Vector3(-2.798f, 0f, 8.952f));
        _goonAgent.SetDestination(new Vector3(-3.66f, 0f, 7.04f));
        StartCoroutine(ChangeLookLeft(_sallosAnimator, 0, 0.5f));
        StartCoroutine(ChangeLookLeft(_eulyssAnimator, 0, 0.45f));
        StartCoroutine(UIManager.UI.PauseAllButtons(1.3f));
    }

    private void Reach()
    {
		_eulyssAnimator.SetTrigger("ReachOut");
    }

    private void Stab()
    {
        _akifAnimator.SetTrigger("Stab");
        _sallosAnimator.SetTrigger("StepBack");
        StartCoroutine(UIManager.UI.PauseAllButtons(0.7f));
    }

    private void Disappear()
    {
        StartCoroutine(FadeAway(5f));
        StartCoroutine(UIManager.UI.PauseAllButtons(5f));
    }

    private IEnumerator FadeAway(float seconds)
    {
        float time = 0f;

        while (_sallosMaterial.GetFloat("_Dissolve_Effect") < 1)
        {
            _sallosMaterial.SetFloat("_Dissolve_Effect", time / seconds);
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(seconds);
    }

    /// <summary>
    /// Changes the LookLeft parameter for Sallos to linearly go to 0 or 1 over
    /// the specified time.
    /// </summary>
    /// <param name="desiredValue">Desired LookLeft value.</param>
    /// <param name="desiredDuration">Time the change should should take.</param>
    /// <returns></returns>
    private IEnumerator ChangeLookLeft(
		Animator animator, float desiredValue, float desiredDuration)
    {
        float timeElapsed = 0f;
        //Determine which formula to use to change lookleft value
        bool increase = animator.GetFloat("LookLeft") < desiredValue;

        if (increase)
        {
            while (animator.GetFloat("LookLeft") < desiredValue)
            {
                animator.SetFloat("LookLeft", timeElapsed / desiredDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (animator.GetFloat("LookLeft") > desiredValue)
            {
                animator.SetFloat("LookLeft", 1 - (timeElapsed / desiredDuration));
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}
