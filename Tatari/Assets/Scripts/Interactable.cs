using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour{

    public abstract string PromptMessage { get; }

    public bool keepMeEnabled;
    public bool isDying;

    public abstract void Interact();
}