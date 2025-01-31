using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnGamePlay : MonoBehaviour
{
    [SerializeField] List<GameObject> toDisable;
    private UIManager uIManager;

    // Start is called before the first frame update
    void Start()
    {
        uIManager = GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(uIManager.CurrentUIMode == UIMode.Gameplay)
        {
            foreach(GameObject gObject in toDisable)
            {
                gObject.SetActive(false);
            }
            
        }
        else if (uIManager.CurrentUIMode == UIMode.Dialogue)
        {
            foreach (GameObject gObject in toDisable)
            {
                gObject.SetActive(true);
            }
        }
    }
}
