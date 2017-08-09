using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * MenuController provides common menu functionality that can be used by 
 * inheriting classes. MenuController is adapted to a menu controlled by
 * key input, not mouse maneuvering. 
 * 
 * MenuController contains arrays of menu items gameObjects and markers
 * (used to indicate selected item) that are disables/enabled when 
 * browsing the menu. 
 * 
 * MenuController contains a list of functions associated with the
 * indicies of corresponding menu item. 
*/

public class MenuController : MonoBehaviour {

    public GameObject[] markers;
    public GameObject[] itemIcons;
    public List<System.Func<int>> actions;
    public int nrMenuItems;

    public Color textDefaultColor;
    Color textSelectedColor;
    int selectedItem;  // 0 indexed
    bool loading;
    bool isExiting;


    public void Init()
    {
        Input.ResetInputAxes();  // Make sure old input does not cause calls to Next() or Prev()
        selectedItem = 0;
        textSelectedColor = new Color(textDefaultColor.r * 0.5f,
                                                    textDefaultColor.g * 0.5f, textDefaultColor.b * 0.5f);
        print(textDefaultColor + " in Init");

        // Make sure the first item is selected and all items have default text color
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

    /* Quits the application. */
    public int Exit()
    {
        if (isExiting) return 0;
        isExiting = true;
        GameController.ExitGame();
        return 0;  // Becasue System.Func requires a return type
    }

    /* Restarts the game from main menu with GameController "showIntro" set to true. */
    public int StartGame()
    {
        if (loading) return 0;
        GameController.instance.showIntro = true;
        GameController.instance.SwitchScene(GameController.INGAME);
        loading = true;
        return 0;  // Becasue System.Func requires a return type
    }

    /* Restarts the INGAME scene with GameController "showIntro" set to false. */
    public int Restart()
    {
        if (loading) return 0;
        GameController.instance.showIntro = false;
        GameController.instance.SwitchScene(GameController.INGAME);
        loading = true;
        return 0;  // Becasue System.Func requires a return type
    }

    /* Select the next menu item. */
    public void Next()
    {
        markers[selectedItem].SetActive(false);
        selectedItem++;
        if (selectedItem == nrMenuItems) selectedItem = 0;
        markers[selectedItem].SetActive(true);

    }

    /* Select the previous menu item. */
    public void Prev()
    {
        markers[selectedItem].SetActive(false);
        selectedItem--;
        if (selectedItem < 0) selectedItem = nrMenuItems - 1;
        markers[selectedItem].SetActive(true);
    }

    /* Calls the function associated with the selected menu item. */
    public void Select()
    {
        itemIcons[selectedItem].transform.Find("Text").GetComponent<Text>().color = textSelectedColor;
        actions[selectedItem]();
    }

    /* Sets the selected menu items text color to the default. */
    public void Deselect()
    {
        itemIcons[selectedItem].transform.Find("Text").GetComponent<Text>().color = textDefaultColor;
    }
}
