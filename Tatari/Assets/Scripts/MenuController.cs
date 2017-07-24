using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public GameObject[] markers;
    public GameObject[] itemIcons;
    public List<System.Func<int>> actions;
    public int nrMenuItems;

    int selectedItem;  // 0 indexed
    bool isRestarting;
    bool isExiting;


    public void Init()
    {
        Input.ResetInputAxes();
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

    void Next()
    {
        markers[selectedItem].SetActive(false);
        selectedItem++;
        if (selectedItem == nrMenuItems) selectedItem = 0;
        markers[selectedItem].SetActive(true);

    }

    void Prev()
    {
        markers[selectedItem].SetActive(false);
        selectedItem--;
        if (selectedItem < 0) selectedItem = nrMenuItems - 1;
        markers[selectedItem].SetActive(true);
    }

    void Select()
    {
        Color color = itemIcons[selectedItem].transform.Find("Text").GetComponent<Text>().color;
        itemIcons[selectedItem].transform.Find("Text").GetComponent<Text>().color = 
                                                                new Color(color.r * 0.5f, color.g * 0.5f, color.b * 0.5f);
        actions[selectedItem]();
    }
}
