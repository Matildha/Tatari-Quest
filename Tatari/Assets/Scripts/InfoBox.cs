using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour {

    public InteractableManager intMan;
    GameObject background;
    string[] messages;
    int nrMsg;
    int currentMsg;

    public void Init()
    {
        background = gameObject.transform.Find("Background").gameObject;
        background.SetActive(false);
    }

    public void DisplayInfo(string[] _messages)
    {
        background.SetActive(true);
        messages = _messages;
        nrMsg = messages.Length;
        currentMsg = 0;
        background.transform.Find("Info").GetComponent<Text>().text = _messages[currentMsg];
        //intMan.ToggleInteractionPrompt(false);
    }

    public void Continue()
    {
        currentMsg++;
        if (currentMsg == nrMsg) Close();
        else background.transform.Find("Info").GetComponent<Text>().text = messages[currentMsg];
    }

    public void Close()
    {
        background.SetActive(false);
    }
}
