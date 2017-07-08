using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour {

    public Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Demon")
        {
            print("Player attacked by demon!");
            player.ChangeFear(Demon.FEAR_INCREASE);
        }
    }
}
