using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class FileIO : MonoBehaviour {

    [HideInInspector] public static FileIO FileIO_instance;
    [HideInInspector] public string DataPathFile;
    public List<string> lines;

    protected string newWord;
    protected string newMeanings;
    protected string newNote;

    private string rotationData;

    protected string tempRandLine;
    protected string randWord;
    protected string randMeanings;
    protected string randNote;

    private string dataTest;

    bool isActiveModify = false;

    void Start()
    {
        FileIO_instance = this;
        DataPathFile = Application.persistentDataPath + "Data2.txt";
        if (!File.Exists(DataPathFile)) {
            using (var fs = File.Create(DataPathFile)) {}
        } else {}
        lines = File.ReadAllLines(DataPathFile).ToList();
    }

    public void _WriteFile(string str) {
        if (CheckExistWord(str)) {
            lines.Add(str);
            File.WriteAllLines(DataPathFile, lines);
            AppManager.instance._AddNewWordsTD();

        }

        //  Show message to UI the word existed
        else Debug.Log("You tauch me this word, remember");
        
    }

    public string _GetRandomWord() {
        return randWord;
    }

    public string _GetRandomNote() {
        return randNote;
    }

    protected void ShowNote() {
        GameObject GO = GameObject.Find("InputFeild_Note_Learing");
        InputField inputField = GO.GetComponent<InputField>();
        inputField.text = _GetRandomNote();

    }

    public void _LearningBtn() {

        //  Unactive learingBtn: On editor
        //  Active learningScreen: On editor

        //  Make randWord
        _RandomWord();
    }

    protected void _RandomWord() {
        int rand = UnityEngine.Random.Range(0, lines.Count);
        while(lines[rand] == "" || lines[rand].Equals(tempRandLine)) {
            rand = UnityEngine.Random.Range(0, lines.Count);
        }
        tempRandLine = lines[rand];
        string[] tempStr = lines[rand].Split('|');
        randWord = tempStr[0];
        randMeanings = tempStr[1];
        randNote = tempStr[2];

        //  Put randWord word to UI
        GameObject showWordFeild = GameObject.Find("Show_word_feild");
        showWordFeild.GetComponent<Text>().text = randWord;

    }

    public void _ModifyBtnOnclick(GameObject Active_GO) {
        isActiveModify = !isActiveModify;
        Active_GO.GetComponent<InputField>().interactable = isActiveModify;
    }

    public void _SaveModifyBtnOnclick(GameObject Active_GO) {
        isActiveModify = false;
        Active_GO.GetComponent<InputField>().interactable = isActiveModify;

        var str = Active_GO.GetComponent<InputField>().text;

        //  Modify in database

        //  Show messasge to UI modify done

        
        for (var i = 0; i < lines.Count;i++) {

            string[] tempStr = lines[i].Split('|');
            if (randNote == tempStr[2]) {
                tempStr[2] = str;
                string newData = tempStr[0].ToString() + '|' + tempStr[1].ToString() + '|' +tempStr[2].ToString();
                lines[i] = newData;

            }
        }
        File.WriteAllLines(DataPathFile, lines);
    }

    public void _NextWordTeachingBtn() {
        GameObject.Find("InputFeild_NewWord_Teaching").GetComponent<InputField>().text = "";
        GameObject.Find("InputFeild_Meanings_Teaching").GetComponent<InputField>().text = "";
        GameObject.Find("InputFeild_Note_Teaching").GetComponent<InputField>().text = "";
    }

    public void _NextWordLearningBtn() {
        //  Clear note feild
        GameObject.Find("InputFeild_Note_Learing").GetComponent<InputField>().text = "";

        //  Clear answer feild
        GameObject.Find("InputFeild_Answer_Learing").GetComponent<InputField>().text = "";
        GameObject.Find("InputFeild_Answer_Learing").GetComponent<InputField>().Select();
        //  Random another word
        _RandomWord();

    }

    public void _AnswerSubmitBtn(GameObject UserAnsFeild_GO) {
        
        //  Check it with its meanings
        string userAnswer = UserAnsFeild_GO.GetComponent<Text>().text.Trim().ToLower();
        string[] meanings = randMeanings.Split(';');
        foreach(var meaning in meanings) {
            if (userAnswer.Equals(meaning)) {
                //  Show note
                ShowNote();
                AppManager.instance._LearnWordsTD();

                //  Show message congrate
                AppManager.instance.Animator_Ans.runtimeAnimatorController = AppManager.instance.animatorCtrls_Ans[0];
                AppManager.instance.Animator_Ans.Play("KLCT_AnsTrue");
                break;
            }
            else {
                //  Remove userAnser from answer feild
                //  Show message to tell user to enter another answer
                AppManager.instance.Animator_Ans.runtimeAnimatorController = AppManager.instance.animatorCtrls_Ans[1];
                AppManager.instance.Animator_Ans.Play("KLCT_AnsFalse");
            }
        }

    }

    public void _SetNewWord(GameObject GO) {

        string str = GO.GetComponent<Text>().text;
        str = str.Trim().ToLower();
        this.newWord = str;

    }

    public void _SetNewMeanings(GameObject GO) {

        string str = GO.GetComponent<Text>().text;
        str = str.Trim().ToLower();
        this.newMeanings = str;        

    }

    public void _SetNewNote(GameObject GO) {
        string str = GO.GetComponent<Text>().text;
        str = str.Trim().ToLower();
        this.newNote = str.Replace('\n',' ');
    }

    public void _AddNewWordToFile() {
        string line = GetNewWord() + '|' + GetNewMeanings() + '|' + GetNewNote();
        _WriteFile(line);
    }

    public void _AnalysisLine(string line) {

        string[] str = line.Split('|');
        string word = str[0];
        string[] meanings = str[1].Split(';');
        string  note = str[2];

        //  Render to UI
        Debug.Log("English word: " + word);
        for(var i = 0; i < meanings.Length; i ++){
            Debug.Log("Meaning " + i + ": " + meanings[i]);
        }
        Debug.Log("Note: " + note);
    }

    protected string GetNewWord() {
        return newWord;
    }

    protected string GetNewMeanings() {
        return newMeanings;
    }

    protected string GetNewNote() {
        return newNote;
    }

    protected bool CheckExistWord(string str) {

        string word = str.Split('|')[0];
        
        foreach(var line in lines) {
            string[] element = line.Split('|');
            if (element[0] == word) {
                return false;
            }
        }
        return true;
    }



}
