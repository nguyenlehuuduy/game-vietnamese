using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SaveManager : MonoBehaviour
{
    public InputField inputPhone;
    public static SaveManager instance { get; private set; }
    // what we want to save
    public string currentPhone;
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            Debug.Log("file exists" + Application.persistentDataPath + "/playerInfo.dat");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerModel infPlayer = (PlayerModel)bf.Deserialize(file);
            Debug.Log(infPlayer.phone);
            currentPhone = infPlayer.phone;
            Debug.Log("Da nhap so dien thoai");
            inputPhone.text = infPlayer.phone;
        }
        else
        {
            Debug.Log("no results !");
        }
    }
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerModel player = new PlayerModel();
        player.phone = currentPhone;

        bf.Serialize(file, player);
        file.Close();
    }
    //update question from server
    public string SaveDataQuestion<T>(JSONDataQuestionClass<T> l_question)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/questionInf.dat");
        bf.Serialize(file, l_question);
        file.Close();
        return Application.persistentDataPath + "/questionInf.dat"; 
    }
    //parse question data from json 
    public JSONDataQuestionClass<T> ParseDataFromJSONQuestion<T>()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/questionInf.dat", FileMode.Open);
        JSONDataQuestionClass<T> l_question = (JSONDataQuestionClass<T>)bf.Deserialize(file);
        return l_question;
    }
}
