using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public static AppManager instance;

    public Animator CreateTargetAnimator;
    public Animator SlideBarAnimator;

    public GameObject Learning;

    private Color32 SlideBarElementColor;
    private Color32 ActiveSlideBarElementColor;
    private Color32 NeededColor;
    
    private Button SlideBarBtn;
    [HideInInspector] public bool isShowingSlideBar;
    [HideInInspector] public bool isShowingCreateTarget;
    [HideInInspector] public bool checkRequiredField;

    [HideInInspector] public string pathTempPerDayFile;
    [HideInInspector] public DateTime tempDate;
    [HideInInspector] public int newWordsAddToday;
    [HideInInspector] public int wordsLearnedToday;
    protected string lineTodayFile;
    protected List<string> linesDataPerDay;

    public Animator Animator_Ans;
    public RuntimeAnimatorController[] animatorCtrls_Ans;

    void Awake()
    {
        instance = this;
        
        isShowingSlideBar = false;
        isShowingCreateTarget = false;
        checkRequiredField = true;

        SlideBarElementColor = new Color32(218,250,255,255);
        ActiveSlideBarElementColor = new Color32(93,234,255,255);
        NeededColor = new Color32(255,211,211,255);

        Learning.GetComponent<Image>().color = ActiveSlideBarElementColor;

        pathTempPerDayFile = Application.persistentDataPath + "TempPerDayData.txt";
        if (!File.Exists(pathTempPerDayFile)) {
            using (var fs = File.Create(pathTempPerDayFile)) {}
            File.WriteAllText(pathTempPerDayFile,DateTime.Today.ToString() + "|0|0");
        } else {}
        linesDataPerDay = File.ReadAllLines(pathTempPerDayFile).ToList();
        _ResetTempDataPerDay();
        
    }

    void Update()
    {
        _Demo();

        if (Input.GetKeyDown(KeyCode.Return)) {
            FileIO.FileIO_instance._AnswerSubmitBtn(GameObject.Find("AnswerFeild"));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            FileIO.FileIO_instance._NextWordLearningBtn();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            _ToggleSlideBar();
        }

    }

    public static AppManager GetInstance() {
        return instance;
    }

    public void _ToggleSlideBar() {
        isShowingSlideBar = !isShowingSlideBar;
        SlideBarAnimator.SetBool("show", isShowingSlideBar);
    }

    public void _ToggleCreateTargetFeild() {

        isShowingCreateTarget = !isShowingCreateTarget;
        CreateTargetAnimator.SetBool("show", isShowingCreateTarget);
    }

    public void _CheckEmptyRequiredField(GameObject GOField) {
        
        InputField inputField = GOField.GetComponent<InputField>();        
        string str = inputField.text;
        str = str.Trim();
        if (str == "" || str == null || str.Equals("")){
            
            //  Show effect to UI
            inputField.image.color = NeededColor;

            //  Show message to UI this feild is required
            checkRequiredField = false;
        }
        else {
            inputField.image.color = Color.white;
            checkRequiredField = true;
        }
    }

    public void _RemoveNeededColor(InputField inpf) {
        inpf.image.color = Color.white; 
    }

    public void _SetNeededColor(InputField inpf) {
        inpf.image.color = NeededColor; 

    }

    public void _HandleActiveColor(GameObject GO) {

        //  Set color unActive for all SlideBarElements 
        GameObject[] SlideBarElements = GameObject.FindGameObjectsWithTag("SlideBarElement");
        foreach(var element in SlideBarElements) {
        
            Image tempImage = element.GetComponent<Image>();
            tempImage.color = SlideBarElementColor;
        }

        //  Set color for the choosen element
        Image image = GO.GetComponent<Image>();
        image.color = ActiveSlideBarElementColor;
    }

    public void _HandleActiveScreen(GameObject GO) {

        //  Set unActive for all screens
        GameObject[] AppScreens = GameObject.FindGameObjectsWithTag("AppScreen");
        foreach (var element in AppScreens) {
            element.SetActive(false);
        }

        //  Set active for the choosen screen
        GO.SetActive(true);
    }

    public void _Demo() {
        if (Input.GetKeyDown(KeyCode.I)) {
            // DateTime dt = DateTime.Today;
            // string str = dt.ToString();
            // Debug.Log("__" + str + "__");
            // DateTime dt2; 
            // DateTime.TryParse(str, out dt2);
            // Debug.Log(dt2.GetType());


        }
    }

    public void _ResetTempDataPerDay() {
        string newDate;
        string newNewWordTD;
        string newWordsLearnTD;

        lineTodayFile = linesDataPerDay[linesDataPerDay.Capacity-1];
        // __NOTE__: lineTodayFile: [0]: date, [1]: newWordsTD, [2]: wordLearnedTD
        DateTime date;
        string[] lineSplit = lineTodayFile.Split('|');
        
        DateTime.TryParse(lineSplit[0],out date);
        if (date != DateTime.Today.Date) {
            newDate = DateTime.Today.Date.ToString();
            newNewWordTD = "0";
            newWordsLearnTD = "0";
            string newLine = newDate + "|" + newNewWordTD + "|" + newWordsLearnTD;
            linesDataPerDay.Add(newLine);
            File.WriteAllLines(pathTempPerDayFile, linesDataPerDay);
        } else {
            newWordsAddToday = int.Parse(lineSplit[1]);
            wordsLearnedToday = int.Parse(lineSplit[2]);
        }
    }

    public void _AddNewWordsTD() {
        newWordsAddToday ++;
        UpdateTempDataPerDay();
    }

    public void _LearnWordsTD() {
        wordsLearnedToday ++;
        UpdateTempDataPerDay();
        UpdateToFirstTarget();
    }

    protected void UpdateTempDataPerDay() {
        string newLine = DateTime.Today.ToString() + "|" + newWordsAddToday + "|" +  wordsLearnedToday;
        linesDataPerDay.RemoveAt(linesDataPerDay.Capacity-1);
        linesDataPerDay.Add(newLine);
        File.WriteAllLines(pathTempPerDayFile, linesDataPerDay);
    }

    protected void UpdateToFirstTarget() {
        if (DateTime.Today == DateTime.Parse(linesDataPerDay[linesDataPerDay.Capacity - 1].Split('|')[0]) ) {
            string todayData = linesDataPerDay[linesDataPerDay.Capacity - 1];
            string firstTargetStillActive;
            var i = 0;
            foreach(var line in TargetManager.instance.lines) {
                string[] elementsInline = line.Split('|');
                i++;
                bool isActiveTarget = bool.Parse(elementsInline[elementsInline.Length-1]);
                if (isActiveTarget) {
                    firstTargetStillActive = line;
                    string[] elements = firstTargetStillActive.Split('|');

                    break;
                } else if (i == TargetManager.instance.lines.Capacity - 1 && !isActiveTarget) {
                    //  Notice to user to create a new target
                    Debug.Log("You have no any target valid now, create a new one!");
                }
            }
        }
    }

}