using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    public InteractableManager intManager;
    public ScrollFactory scrollFact;

    public float minRoomHeight;
    public GameObject[] demonStart;  // Control points for possible demon horizontal start positions
    public GameObject[] demonBezCP;  // Control points for demon bezier curve movement (MAX SIZE 2)

	void Start () {
        intManager.Init();
        scrollFact.Init();
	}
}