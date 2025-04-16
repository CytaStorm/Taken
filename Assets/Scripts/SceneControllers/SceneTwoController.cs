using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class SceneTwoController : CutsceneController
{
    // Actors
    [SerializeField] private GameObject _sallos;

    // Actor Components
    private NPCScript _sallosScript;
    private NavMeshAgent _sallosAgent;
    private Animator _sallosAnimator;

    //Interactable positions
    [SerializeField] private GameObject _deadTree;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        //Hook up events
        _flagNames.Add("logQuest");
        _flagNames.Add("kindlingQuest");

        CreateEventFlags();

        _eventFlags[1].onValueChange += delegate { SallosGoToLog(); };
        //_eventFlags[1].onValueChange += delegate

        _sallosScript = _sallos.GetComponent<NPCScript>();
        _sallosAgent = _sallos.GetComponent<NavMeshAgent>();
        _sallosAnimator = _sallos.GetComponent<Animator>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    private void SallosGoToLog()
    {
        _sallosAgent.speed = 3f;
        _sallosAgent.SetDestination(new Vector3(32.81f, 0f, 38.47f));

        StartCoroutine(_sallosScript.DisableInteractionForDuration(3));
        StartCoroutine(ArriveAtLog());

        //while (_sallos.transform.rotation.y != -97.8)
        //{
        //    _sallos.transform.rotation = Quaternion.RotateTowards(_sallos.transform.rotation, Quaternion.Euler(0, -97.8f, 0), 180);
        //}
        //_sallosScript.InteractionPoint.transform.position = new Vector3();
    }
    
    private IEnumerator ArriveAtLog()
    {
        Transform sallosMesh = _sallosScript.Mesh.transform;

        //while (sallosMesh.position.x != _sallosAgent.destination.x && 
        //    sallosMesh.transform.position.z != _sallosAgent.destination.z)
        while (Vector3.Distance(sallosMesh.position, _sallosAgent.destination) > 0.1f)
        {
            yield return null;
        }

        _sallosAgent.isStopped = true;

        _sallosScript.transform.rotation = 
            Quaternion.Euler(_sallosScript.transform.rotation.x, 40, _sallosScript.transform.rotation.z);

        Quaternion q = Quaternion.LookRotation(
            _deadTree.transform.position - sallosMesh.transform.position);

        while(Quaternion.Angle(sallosMesh.transform.rotation, q) > 0)
        {
            sallosMesh.transform.rotation = 
                Quaternion.RotateTowards(sallosMesh.transform.rotation, q, 50 * Time.deltaTime);
            yield return null;
        }

    }
}
