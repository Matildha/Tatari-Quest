using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScrollFactory : MonoBehaviour {

    public GameObject orgScroll;
    public WorldManager worldMan;

    List<string> descriptions;
    List<string> colors;  // TODO: Change to list of images or indices for images!
    Vector3[] occupiedPositions = new Vector3[Inventory.MAX_NR_SCROLLS];


    public void Init()
    {   descriptions = new List<string>();
        colors = new List<string>(new string[] { "Red", "Blue" });
    }

    public void CreateScrolls()
    {
        LoadScrollInfo();

        int content;
        int color;

        Random.InitState(System.DateTime.Now.Millisecond);
        int i = 0;
        int whileSaver = 0;

        while(i < Inventory.MAX_NR_SCROLLS && whileSaver < 50)
        {
            whileSaver++;

            Scroll.ScrollInfo info = new Scroll.ScrollInfo();
            
            // Set a random position
            int areaID = Random.Range(0, worldMan.numberOfWorldAreas);
            WorldArea worldArea = worldMan.worldAreas[areaID];
            GameObject rndPosition = worldArea.scrollPositions[Random.Range(0, worldArea.nrScrollPositions)];

            // If position has already been assigned, go back and choose a new one
            if (System.Array.Exists<Vector3>(occupiedPositions, element => element == rndPosition.transform.position))
                continue;

            occupiedPositions[i] = rndPosition.transform.position;

            // Add random content and random color        
            content = Random.Range(0, Inventory.MAX_NR_SCROLLS - i);
            color = Random.Range(0, Inventory.MAX_NR_SCROLLS - i);
            info.content = descriptions[content];
            descriptions.RemoveAt(content);
            info.color = colors[color];
            colors.RemoveAt(color);

            // Add properties to new scroll and add the to InteractableManager
            // if random position has not already been occupied
            
            GameObject newScrollGameObj = Instantiate(orgScroll) as GameObject;
            Scroll newScroll = newScrollGameObj.GetComponent<Scroll>();

            newScroll.info = info;
            newScroll.transform.position = rndPosition.transform.position;
            newScroll.transform.rotation = rndPosition.transform.rotation;
            newScroll.gameObject.transform.SetParent(transform.parent);
            newScroll.gameObject.SetActive(true);
            newScroll.gameObject.layer = 9;

            worldMan.worldAreas[areaID].interactables.AddLast(newScroll);

            i++;

            print("Created scroll " + newScroll + " " + newScroll.info.content + " " + 
                                           newScroll.info.color + " " + newScroll.transform.position);
        }
    }

    void LoadScrollInfo()
    {
        FileStream stream = new FileStream("Assets/TextFiles/scroll-info.txt", FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(stream);

        for (int i = 0; i < Inventory.MAX_NR_SCROLLS; i++)
        {
            descriptions.Add(reader.ReadLine());
        }

        reader.Close();
        stream.Close();
    }
}