using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Scroll inherits from Interactable and defines 
 * the struct ScrollInfo used to contain the scroll
 * content and its associated color. 
 * 
 * Scroll's Interact method destroys the gameObject this
 * script is attached to, removes it from InteractableManager 
 * and adds the ScrollInfo "info" to player inventory. 
*/

public class Scroll : Interactable {

    public struct ScrollInfo
    {
        public string content;
        public ScrollFactory.ScrollColors color;
    }


    const string PROMPT_MSG = "Press E to pick up scroll";

    public override string PromptMessage { get { return PROMPT_MSG; } }

    public ScrollInfo info;


    public override void Interact()
    {
        //print("Picking up scroll with content " + info.content + " " + info.color);
        InteractableManager intManager = GameObject.Find("Interactables").GetComponent<InteractableManager>();
        // So this scroll is not updated in InteractableManager
        intManager.RemoveInteractable(this);  
        intManager.ResetInRangeInteractable();

        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.inventory.AddScroll(this.info);

        Destroy(this.gameObject);
    }
}
