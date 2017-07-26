using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public GameObject[] markers;
    public GameObject[] itemIcons;
    public List<System.Func<int>> actions;
    public int nrMenuItems;

    public Color textDefaultColor;
    Color textSelectedColor;
    int selectedItem;  // 0 indexed
    bool isRestarting;
    bool isExiting;


    public void Init()
    {
        Input.ResetInputAxes();
        selectedItem = 0;
        textSelectedColor = new Color(textDefaultColor.r * 0.5f,
                                                    textDefaultColor.g * 0.5f, textDefaultColor.b * 0.5f);
        print(textDefaultColor + " in Init");

        for(int i=0; i<nrMenuItems; i++)
        {
            if (i == 0)
            {
                markers[i].SetActive(true);
            }
            else
            {
                markers[i].SetActive(false);
            }
            itemIcons[i].transform.Find("Text").GetComponent<Text>().color = textDefaultColor;
        }   
    }

    public int Exit()
    {
        if (isExiting) return 0;
        Application.Quit();
        isExiting = true;
        return 0;  // Becasue System.Func requires a return type
    }

    public int Restart()
    {
        if (isRestarting) return 0;
        GameController.instance.SwitchScene(GameController.INGAME);
        isRestarting = true;
        return 0;  // Becasue System.Func requires a return type
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Next();
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Prev();
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            Select();
        }
	}

    public void Next()
    {
        markers[selectedItem].SetActive(false);
        selectedItem++;
        if (selectedItem == nrMenuItems) selectedItem = 0;
        markers[selectedItem].SetActive(true);

    }

    public void Prev()
    {
        markers[selectedItem].SetActive(false);
        selectedItem--;
        if (selectedItem < 0) selectedItem = nrMenuItems - 1;
        markers[selectedItem].SetActive(true);
    }

    public void Select()
    {
        itemIcons[selectedItem].transform.Find("Text").GetComponent<Text>().color = textSelectedColor;
        actions[selectedItem]();
    }

    public void DeSelect()
    {
        itemIcons[selectedItem].transform.Find("Text").GetComponent<Text>().color = textDefaultColor;
    }
}
