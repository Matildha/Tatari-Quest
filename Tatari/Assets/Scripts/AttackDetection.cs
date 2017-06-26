using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Demon")
        {
            print("Player attacked by demon!");

        }
        //print("Collision trigger enter!");
    }
}
