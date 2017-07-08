using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threshold : MonoBehaviour {

    WorldManager worldMan;
    public int areaID;

    private void Start()
    {
        worldMan = GameObject.Find("Environment").GetComponent<WorldManager>();
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            worldMan.SwitchArea(areaID);
            print("Changed world area to " + areaID);
        }
    }
}
