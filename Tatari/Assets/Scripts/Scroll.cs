using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : Interactable {

    const string PROMPT_MSG = "Press E to pick up scroll";

    public override string PromptMessage { get { return PROMPT_MSG; } }

    public struct ScrollInfo
    {
        public string content;
        public string color;  // TODO: change to index for scroll image in inventory
    }

    public ScrollInfo info;


    public override void Interact()
    {
        print("Picking up scroll with content " + info.content + " " + info.color);
        InteractableManager intManager = GameObject.Find("Interactables").GetComponent<InteractableManager>();
        intManager.RemoveInteractable(this);  // So this scroll is not updated in InteractableManager
        intManager.ResetInRangeInteractable();

        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.inventory.AddScroll(this.info);

        Destroy(this.gameObject);
    }
}