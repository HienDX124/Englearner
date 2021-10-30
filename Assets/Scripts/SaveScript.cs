using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AppData))]
public class SaveScript : MonoBehaviour
{

    string dataTest;
    private string savePath;

    void Start()
    {
        dataTest = "DatatestAdd";
        savePath = Application.persistentDataPath + "DataTest5.txt";
        Debug.Log(Application.persistentDataPath);
    }

    public void _SaveData() {

        var Data = dataTest;
        Saver saver = new Saver() {
            saveStr = dataTest
        };
        var binaryFormatter = new BinaryFormatter();
        if (!File.Exists(savePath)) {
            using (var fs = File.Create(savePath)) {
                binaryFormatter.Serialize(fs, saver);
            }
            Debug.Log("Creat new dataFile!");
        }
        else {
            using (var fs = File.OpenWrite(savePath)) {
                binaryFormatter.Serialize(fs, saver);
            }
            Debug.Log("Open available dataFile!");
        }
        Debug.Log("Data saved!");
    }

    public void _LoadData() {
        if (File.Exists(savePath)) {
            Saver saver;
            var binaryFormatter = new BinaryFormatter();
            using (var fs = File.Open(savePath, FileMode.Open)) {
                saver = (Saver)binaryFormatter.Deserialize(fs);
            }
        
        GameObject.Find("Str").GetComponent<Text>().text = dataTest;

        Debug.Log("Data loaded!");
        }
        else  {
            Debug.Log("Data doesn't exist!");
        }
    }

}
