using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppData : MonoBehaviour
{

    public string GameStr { get; set;}

    [SerializeField] private Text textInt;
    [SerializeField] private Text textStr;

    public void _GenerateNewData() {
        GameStr = textStr.text;
        ShowData();
    }

    public void ShowData() {
        textStr.text = GameStr;
    }

}
