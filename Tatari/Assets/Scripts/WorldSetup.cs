using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSetup : MonoBehaviour {

    public InteractableManager intManager;
    public ScrollFactory scrollFact;

	void Start () {
        intManager.Init();
        scrollFact.Init();
	}
}
