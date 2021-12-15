using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TodaySumary : MonoBehaviour
{
    public static TodaySumary instance;
    [HideInInspector] public List<string> lines;
    private GameObject TodayDate;
    private GameObject WordAdded;
    private GameObject WordLearned;
    private GameObject CompareYesterday;

    private string DataLine;
    private string[] Data;

    void Start()
    {
        instance = this;
        TodayDate = gameObject.transform.Find("Label").gameObject;
        WordAdded = gameObject.transform.Find("WordAdded").gameObject;
        WordLearned = gameObject.transform.Find("WordLearned").gameObject;
        UpdateData();
    }

    public void UpdateData()
    {
        lines = AppManager.instance.linesDataPerDay;
        string lastLine = lines[lines.Capacity - 1];
        if (DateTime.Today == DateTime.Parse(lastLine.Split('|')[0]))
        {
            DataLine = lastLine;
        }
        else
        {
            DataLine = DateTime.Today + "|" + 0 + "|" + 0;
        }
        Data = DataLine.Split('|');
    }
    public void SetContent()
    {

        TodayDate.transform.GetComponentInChildren<Text>().text = "Sumary " + Data[0].Split(' ')[0];
        WordAdded.transform.GetComponentInChildren<Text>().text = "Words added\t : " + Data[1];
        WordLearned.transform.GetComponentInChildren<Text>().text = "Words learned\t : " + Data[2];
    }

}
