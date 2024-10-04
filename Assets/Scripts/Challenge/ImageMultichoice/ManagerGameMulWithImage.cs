using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;


[Serializable]
public class ManagerGameMulWithImage : MonoBehaviour
{
    // for UI
    public List<Button> btnOptions ;
    public List<Image> imagesToLoad;
    private List<string> options = new List<string>();
    private List<string> imagePaths = new List<string>();
    public List<string> answers = new List<string>();
    public TextMeshProUGUI desciption = new TextMeshProUGUI();

    //for logic game
    public List<TextMeshProUGUI> textMeshProList =  new List<TextMeshProUGUI>();
    public List<(string path, string answer)> pathAnswerPairs = new List<(string, string)>();
    public ManagerPointAndTime managerPoint;
    public int numDrop = 0;
    public List<GameObject> panelQS;
    private bool finish = false;
    public bool Finish
    {
        get { return finish; }
        set { finish = value; }
    }
    void Start()
    {
        managerPoint = FindAnyObjectByType<ManagerPointAndTime>();
    }
    public void play(QuestionModel<string> qs)
    {
        if (qs != null)
        {
            
            setDes(qs);
            setAnswer(qs);
            setOptions(qs);
            SetImagePaths(qs);
            loadOptiontoButton(qs);
            LoadImagesAndAnswer(qs);
        }
        else
        {
            Debug.Log(1);
        }
    }
  
    public void setDes(QuestionModel<string> question)
    {
        if (desciption !=null)
        {
            desciption.text = question.content.des;

        }
        else
        {
            Debug.Log(2);
        }
    }
    public void setOptions(QuestionModel<string> question)
    {
        options.Clear();
        for (int i = 0; i < 3; i++)
        {
            options.Add(question.options[i]);
        }
    }
    public void loadOptiontoButton(QuestionModel<string> question)
    {
        setOptions(question);
        if (btnOptions.Count != options.Count)
        {
            Debug.LogError("Number of string and button does not match.");
            return;
        }
        System.Random random = new System.Random();
        options = options.OrderBy(x => random.Next()).ToList();

        for (int i = 0; i < 3; i++)
        {
            if (i < options.Count)
            {
                string option = options[i];
                TextMeshProUGUI tmp = btnOptions[i].GetComponentInChildren<TextMeshProUGUI>();
                tmp.text = option;
            }
            else
            {
                Debug.LogError("Not enough options provided for the number of buttons.");
            }
        }
    }
    public void LoadImagesAndAnswer(QuestionModel<string> question)
    {
        SetImagePaths(question);
        setAnswer(question);
        for (int i = 0; i < imagePaths.Count; i++)
        {
            pathAnswerPairs.Add((imagePaths[i], answers[i]));
        }
        System.Random random = new System.Random();
        pathAnswerPairs = pathAnswerPairs.OrderBy(x => random.Next()).ToList();
        for (int i = 0; i < imagesToLoad.Count; i++)
        {
            if (i < pathAnswerPairs.Count)
            {
                string imagePath = pathAnswerPairs[i].path;
                Sprite loadedImage = Resources.Load<Sprite>(imagePath);

                if (loadedImage != null)
                {
                    imagesToLoad[i].sprite = loadedImage;
                }
                else
                {
                    Debug.LogError("Image not found or loaded: " + imagePath);
                }

                string answer = pathAnswerPairs[i].answer;
                textMeshProList[i].text = answer;
                textMeshProList[i].alpha = 0;
            }
            else
            {
                Debug.LogError("Not enough path-answer pairs provided for the number of images to load.");
            }
        }
    }
    public void setAnswer(QuestionModel<string> question)
    {
        answers.Clear();
        answers = question.answer.Split("|", StringSplitOptions.None).ToList();
        
    }
    public void SetImagePaths(QuestionModel<string> question)
    {
        // Clear existing paths and add new paths
        imagePaths.Clear();
        for (int i = 0; i < question.content.images.Length; i++)
        {
            imagePaths.Add(question.content.images[i]);
        }

    }
    public bool isFinish()
    {
        checkAnswer();
        return finish;
    }
    public void checkAnswer()
    {
        int count = 0;
        string option = "";
        string answer = "";
        bool isRight = true;
        for (int i = 0;i<panelQS.Count;i++)
        {
            if (panelQS[i].transform.childCount == 4)
            {
                count++;
                option = panelQS[i].transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text.Trim().ToLower();
                answer = panelQS[i].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text.Trim().ToLower();
                if (option != answer)
                {
                    isRight = false;
                }
            }          
        }
        if (count == 3 && isRight)
        {
            managerPoint.addPoint();
            finish = true;
        }
    }
    
}