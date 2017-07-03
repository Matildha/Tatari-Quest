using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : Interactable {

    public struct ScrollInfo
    {
        string content;
        int color;  // index for scroll image in inventory
    }

    public ScrollInfo info;

    public Scroll(ScrollInfo _info)
    {
        this.info = _info;
    }

    public override void InteractPrompt(bool toggle)
    {
        if (toggle)
        {
            print("Press E to pick up scroll");
        }
    }

    public override void Interact()
    {
        print("Picking up scroll");
        LinkedList<Interactable> interactables = transform.parent.GetComponent<InteractableManager>().interactables;
        interactables.Remove(this);  // So this scroll is not looped through in InteractableManager
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.inventory.AddScroll(this.info);
        Destroy(this.gameObject);
    }

}
