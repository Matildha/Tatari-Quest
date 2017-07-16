using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour {

    public InteractableManager intMan;
    GameObject background;

    private void Start()
    {
        background = gameObject.transform.Find("Background").gameObject;
        background.SetActive(false);
    }

    public void DisplayInfo(string info)
    {
        background.SetActive(true);
        background.transform.Find("Info").GetComponent<Text>().text = info;
        intMan.ToggleInteractionPrompt(false);
    }

    public void Close()
    {
        background.SetActive(false);
    }
}
