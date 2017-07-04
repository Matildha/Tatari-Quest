using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScrollFactory : MonoBehaviour {

    public GameObject orgScroll;
    public InteractableManager intManager;

    List<string> descriptions;
    List<Vector3> positions;
    List<string> colors;  // TODO: Change to list of images or indices for images!


    public void Init()
    {   descriptions = new List<string>();
        positions = new List<Vector3>(new Vector3[] { new Vector3(3.28f, 0.33f, 1.13f), new Vector3(8.94f, 0.33f, -2.11f) });
        colors = new List<string>(new string[] { "Red", "Blue" });

        LoadScrollInfo();
        CreateScrolls();
    }

    void LoadScrollInfo()
    {
        FileStream stream = new FileStream("Assets/TextFiles/scroll-info.txt", FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(stream);

        for(int i=0; i < Inventory.MAX_NR_SCROLLS; i++)
        {
            descriptions.Add(reader.ReadLine());
        }

        reader.Close();
        stream.Close();
    }

    void CreateScrolls()
    {
        int content;
        int color;

        Random.InitState(System.DateTime.Now.Millisecond);

        for(int i=0; i < Inventory.MAX_NR_SCROLLS; i++)
        {
            Scroll.ScrollInfo info = new Scroll.ScrollInfo();

            // Add random content and random color        
            content = Random.Range(0, Inventory.MAX_NR_SCROLLS - i);
            color = Random.Range(0, Inventory.MAX_NR_SCROLLS - i);
            info.content = descriptions[content];
            descriptions.RemoveAt(content);
            info.color = colors[color];
            colors.RemoveAt(color);

            // Set a random position
            int position = Random.Range(0, Inventory.MAX_NR_SCROLLS - i);

            // Add properties to new scroll and add the to InteractableManager
            GameObject newScrollGameObj = Instantiate(orgScroll) as GameObject;
            Scroll newScroll = newScrollGameObj.GetComponent<Scroll>();

            newScroll.info = info;
            newScroll.transform.position = positions[position];
            positions.RemoveAt(position);
            newScroll.gameObject.transform.SetParent(transform.parent);
            newScroll.gameObject.SetActive(true);
            newScroll.gameObject.layer = 9;

            intManager.AddInteractable(newScroll);

            print("Created scroll " + newScroll + " " + newScroll.info.content + " " + 
                                            newScroll.info.color + " " + newScroll.transform.position);
        }
    }
}