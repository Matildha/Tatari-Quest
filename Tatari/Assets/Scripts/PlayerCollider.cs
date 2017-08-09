using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PlayerCollider should be attached to the gameObject of the player's physical collider. 
 * PlayerCollider will recognize trigger collissions with objects tagged "Demon", this
 * will cause a call to the player's FearMeter reference with the current difficult 
 * level Demon's fear increase. 
*/

public class PlayerCollider : MonoBehaviour {

    public Player player;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Demon")
        {
            print("Player attacked by demon!");
            player.fearMeter.ChangeFear((float) Demon.fearIncreaseLvls[GameController.instance.diffLvl]);
        }
    }
}
