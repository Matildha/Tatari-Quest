using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour {

    public InteractableManager intMan;
    public GameObject background;
    List<string> messages;

    public void Init()
    {
        //background = gameObject.transform.Find("Background").gameObject;
        //background.SetActive(false);
        messages = new List<string>();
    }

    public void DisplayInfo(string[] _messages)
    {
        if (_messages.Length == 0) return;

        background.SetActive(true);
        foreach (string msg in _messages)
        {
            messages.Add(msg);
        }
        // Check if any old messages are left
        if (messages.Count == _messages.Length)
        {
            background.transform.Find("Info").GetComponent<Text>().text = messages[0];
            messages.RemoveAt(0);
        }
        //intMan.ToggleInteractionPrompt(false);
    }

    public void Continue()
    {
        if (messages.Count == 0)
        {
            Close();
        }
        else
        {
            background.transform.Find("Info").GetComponent<Text>().text = messages[0];
            messages.RemoveAt(0);
        }
    }

    public void Close()
    {
        background.SetActive(false);
    }

    public void Show()
    {
        background.SetActive(true);
    }
}
