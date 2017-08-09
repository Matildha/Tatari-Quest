using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Threshhold contains a OnTriggerExit() implementaion
 * to notify WorldManager that a collider tagged "Player" 
 * has entered a new area of index "areaID". 
*/

public class Threshold : MonoBehaviour {

    public int areaID;

    WorldManager worldMan;


    private void Start()
    {
        worldMan = GameObject.Find("Environment").GetComponent<WorldManager>();
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            worldMan.SwitchArea(areaID);
            //print("Changed world area to " + areaID);
        }
    }
}
