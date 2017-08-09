using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The abstract class implemented by the game world objects the player can 
 * interact with. 
 * 
 * Objects inheriting from this class are mainly managed by InteractableManager. 
*/

public abstract class Interactable : MonoBehaviour {

    public abstract string PromptMessage { get; }

    public bool keepMeEnabled;
    public bool isDying;

    public abstract void Interact();
}
