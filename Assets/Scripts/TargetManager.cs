using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TargetManager : MonoBehaviour
{
    
    public GameObject ItemTargetContainer;
    public GameObject ItemTargetPrefab;
    public GameObject EmptyGO;

    [HideInInspector] public static TargetManager instance;
    [HideInInspector] public string pathFileTarget;
    public List<string> lines;
    void Start()
    {
        instance = this;

        pathFileTarget = Application.persistentDataPath + "AllTarget.txt";
        if (!File.Exists(pathFileTarget)) {
            using (var fs = File.Create(pathFileTarget)) {}
        } else {}

        lines = File.ReadAllLines(pathFileTarget).ToList();
        _RenderTarget();
    }

    void Update()
    {

    }

    public void _CreatNewTarget() {
        bool[] checkRequireds = new bool[3];
        
        Target target = new Target() {};
        
        //  Set target info
        var nameGet = GameObject.Find("InputField_Name_Target").GetComponent<InputField>();
        if (!nameGet.text.Equals("")) {
            target._SetTarName(nameGet.text.Trim());
            checkRequireds[0] = true;
        } else { 
            checkRequireds[0] = false;
            AppManager.instance._SetNeededColor(nameGet);
            Debug.Log("Target name mustn't be null!"); 
        }

        var targetGet = GameObject.Find("InputField_NumOfWords_Target").GetComponent<InputField>();
        if (!targetGet.text.Equals("") || !(targetGet.text == null)) {
            int temp;
            if (int.TryParse(targetGet.text, out temp)) {
                target._SetNumOfWords(int.Parse(targetGet.text.Trim()));
                checkRequireds[1] = true;
            }
            else { 
                checkRequireds[1] = false;
                AppManager.instance._SetNeededColor(targetGet);
                Debug.Log("Target must be a number"); 
            }
        } else { Debug.Log("NumOfWords is needed!"); }

        var timeLimGet = GameObject.Find("InputField_TimeLim_Target").GetComponent<InputField>();
        if (!timeLimGet.text.Equals("") || !(timeLimGet.text == null)) {
            int temp;
            if (int.TryParse(timeLimGet.text, out temp)) {
                target._SetTimeLim(int.Parse(timeLimGet.text));
                checkRequireds[2] = true;

            } else { 
                checkRequireds[2] = false;
                AppManager.instance._SetNeededColor(timeLimGet);
                Debug.Log("Time limit must be a number (days)"); 
            }
        } else { Debug.Log("Time limit is needed!"); }

        string reasonGet = GameObject.Find("InputField_Reason_Target").GetComponent<InputField>().text; 
        target._SetReason(reasonGet.Trim());
        
        target.remainingDay = target.timeLim;

        target.startDay = System.DateTime.Today;

        //  Write the target info to file targetManager
        if (checkRequireds[0] && checkRequireds[1] && checkRequireds[2]) {
            string line = target._GetTargetName() + "|" + target._GetNumberOfWords() + "|" + target._GetTimeLim() + "|" + target._GetTargetReason() + "|" + target.currentWordsLearned  + "|" + target.remainingDay + "|" + target.startDay + "|" + target.isActive;
            if (!_CheckTarNameExist(line,lines)) {
                _WriteTarInfoToFile(line);
                _ResetCreateField();
                AppManager.instance._ToggleCreateTargetFeild();

                //  Destroy all item_terget
                foreach (Transform child in ItemTargetContainer.transform) {
                    GameObject.Destroy(child.gameObject);
                }
                RectTransform rectTransform = ItemTargetContainer.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x,0);

                //  Render all target again
                _RenderTarget();

            } else {
                AppManager.instance._SetNeededColor(nameGet);
                Debug.Log("Target name has existed!");
            }

        }
        else {
            Debug.Log("Create failed!");
        }
    }

    public bool _CheckTarNameExist(string str, List<string> lines) {
        string name = str.Split('|')[0];
        
        foreach(var line in lines) {
            string[] element = line.Split('|');
            if (element[0] == name) {
                return true;
            }
        }
        return false;
    }

    public void _WriteTarInfoToFile(string line) {
        lines.Add(line);
        File.WriteAllLines(pathFileTarget, lines);
    }

    public void _ResetCreateField() {
        GameObject.Find("InputField_Name_Target").GetComponent<InputField>().text = "";
        GameObject.Find("InputField_NumOfWords_Target").GetComponent<InputField>().text = "";
        GameObject.Find("InputField_TimeLim_Target").GetComponent<InputField>().text = "";
        GameObject.Find("InputField_Reason_Target").GetComponent<InputField>().text = "";
        
    }

    public void _RenderTarget() {
        for (int i = 0; i < lines.Capacity; i++) {
            string[] parts = lines[i].Split('|');

            //  Instantiate Target_GO
            GameObject temp = Instantiate(ItemTargetPrefab);
            temp.transform.SetParent(ItemTargetContainer.transform);

            //  Change show field height
            RectTransform rectTransform = ItemTargetContainer.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + 210);
            rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y-105, rectTransform.position.z);

            //  Handle target data
            ScriptTargetGO script = temp.GetComponent<ScriptTargetGO>();

            script.text_process.text = parts[4] + '/' + parts[1];

            script.targetName.text = parts[0];

            var tempRemainingDay = (float.Parse(parts[2]) - (DateTime.Today.DayOfYear - DateTime.Parse(parts[6]).DayOfYear));
            script.text_remaining_time.text = tempRemainingDay.ToString();

            Vector3 tempPosProcess = script.image_process.GetComponent<RectTransform>().localScale;
            script.image_process.GetComponent<RectTransform>().localScale = new Vector3(float.Parse(parts[4]) / float.Parse(parts[1]) , tempPosProcess.y, tempPosProcess.z);

            Vector3 tempPosRemainingTime = script.image_remaining_time.GetComponent<RectTransform>().localScale;
            script.image_remaining_time.GetComponent<RectTransform>().localScale = new Vector3(tempRemainingDay / float.Parse(parts[2]), tempPosRemainingTime.y, tempPosRemainingTime.z);
        }
    }

    public void _UpdateAllTargetData() {
        //  Write file what change in each target

    }

    public void UpdateDataActiveTarget(string tarName) {
        foreach (var line in lines) {
            string[] str = line.Split('|');
            if (str[0] == tarName) {
                
            }
        } 

    }

}
