using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{

    public float showDuration;
    public GameObject mess_GO;
    public string message;

    public void AddNotice()
    {
        mess_GO.GetComponent<Text>().text = message;
    }

}
