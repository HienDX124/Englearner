using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target {   

    public string name;
    public int numberOfWords;
    public int timeLim;
    public string reason;
    public DateTime startDay;

    public int currentWordsLearned = 0;
    public int remainingDay;

    public bool isActive = false;
    public void _SetTarName(string vName) {
        name = vName;    
    }

    public void _SetNumOfWords(int vNum) {
        numberOfWords = vNum;
    }

    public void _SetTimeLim(int vDay) {
        timeLim = vDay;
    }

    public void _SetReason(string vReason) {
        reason = vReason;
    }

    public string _GetTargetName(){
        return name;
    }

    public int _GetNumberOfWords() {
        return numberOfWords;
    }

    public int _GetTimeLim() {
        return timeLim;
    }

    public string _GetTargetReason() {
        return reason;
    }


}


