using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class ManagerGamePlay : MonoBehaviour
{
    private ManagerUIChallenge managerUI;
    private List<QuestionModel<string>> questionList = new List<QuestionModel<string>>();
    public ManagerGameMulWithImage gameMulWithImage;
    public ManagerPointAndTime managerPointAndTime;
    
    
    public static int countIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        managerUI = GetComponent<ManagerUIChallenge>();
        gameMulWithImage = FindAnyObjectByType<ManagerGameMulWithImage>();
        managerPointAndTime = FindAnyObjectByType<ManagerPointAndTime>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadListQuestionFromJson()
    {
        string filePath = "Assets/model.json"; // 1 list cau hoi
        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            this.questionList = JsonConvert.DeserializeObject<List<QuestionModel<string>>>(jsonContent);
        }
        else
        {
            Debug.LogError("JSON file not found at path: " + filePath);
        }
    }
    public QuestionModel<string> getQuestion()
    {

        Debug.Log(countIndex);

        if (questionList.Count == 0)
        {
            LoadListQuestionFromJson();
        }

        countIndex += 1;
        if(countIndex == 12)
        {
            countIndex = 0;
        }
        return questionList[countIndex/2]; //return random question
    }
}
