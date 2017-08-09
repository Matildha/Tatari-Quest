using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * WorldArea contains collections of information and gameObjects associated with the area of index "ID". 
 * 
 * WorldArea is intented to represent a physical area in the game world where the below listed gameObject:s
 * are positioned. 
*/

public class WorldArea : MonoBehaviour {

    public int ID;

    public LinkedList<Interactable> interactables;  // Linked list to enable fast removal
    public GameObject[] scrollPositions;
    public int nrScrollPositions;
    public GameObject[] victimPositions;
    public int nrVictimPositions;
    public GameObject[] demonStart;  // Control points to randomize demon start position (SIZE 2)
    public GameObject[] demonBezCP;  // Control points for demon bezier curve movement (SIZE 2)
    public Door[] doors;
}
