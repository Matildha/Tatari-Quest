using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : Interactable {

    public string promptMsg = "Press E to pick up Scroll";

    public override string PromptMessage { get { return promptMsg; } }

    public struct ScrollInfo
    {
        public string content;
        public string color;  // TODO: change to index for scroll image in inventory
    }

    public ScrollInfo info;


    /*public override void InteractPrompt(bool toggle)
    {
        if (toggle)
        {
            print("Press E to pick up scroll");
        }
    }*/

    public override void Interact()
    {
        print("Picking up scroll with content");
        InteractableManager intManager = transform.parent.GetComponent<InteractableManager>();
        intManager.RemoveInteractable(this);  // So this scroll is not updated in InteractableManager
        intManager.ResetInRangeInteractable();

        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.inventory.AddScroll(this.info);

        Destroy(this.gameObject);
    }
}