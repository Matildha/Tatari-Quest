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

        for (int i=_messages.Length-1; i >= 0; i--)
        {
            messages.Insert(0, _messages[i]);
        }

        background.transform.Find("Info").GetComponent<Text>().text = messages[0];
    }

    public void Continue()
    {
        if(messages.Count == 0)
        {
            return;
        }
        else if (messages.Count == 1)
        {
            messages.RemoveAt(0);
            Close();
        }
        else
        {
            messages.RemoveAt(0);
            background.transform.Find("Info").GetComponent<Text>().text = messages[0];
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
