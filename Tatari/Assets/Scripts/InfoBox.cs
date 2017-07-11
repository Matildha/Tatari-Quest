using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour {

    public InteractableManager intMan;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void DisplayInfo(string info)
    {
        gameObject.SetActive(true);
        gameObject.transform.Find("Info").GetComponent<Text>().text = info;
        intMan.ToggleInteractionPrompt(false);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
