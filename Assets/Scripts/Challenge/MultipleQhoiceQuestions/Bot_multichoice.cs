using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Bot_multichoice : MonoBehaviour
{
    public ManagerPointAndTime managerPoint;
    public ManagerPointAndTime managerPointAndTime;


    public Text contentQuestion;
    public Text QuetionIndeces;

    public Text textContentA;
    public Text textContentB;
    public Text textContentC;
    public Text textContentD;

    public Button btnA;
    public Button btnB;
    public Button btnC;
    public Button btnD;

    public Sprite choiseOption;
    public Sprite currentOption;

    public string answer;
    public string botAnswer;
    public bool finish = false;

    public List<string> boxRandom = new List<string>();
    public List<string> percenRandom = new List<string>();
    public List<Button> listButton = new List<Button>();

    public int botSmart = 1;

    [SerializeField]
    public void play(QuestionModel<string> question)
    {
        loadQuestionCurrenr(question);
        Debug.Log(3);
        //btnA.onClick.AddListener(() => HandleOnclickButtonOption("A", textContentA, btnA, question));
        //btnB.onClick.AddListener(() => HandleOnclickButtonOption("B", textContentB, btnB, question));
        //btnC.onClick.AddListener(() => HandleOnclickButtonOption("C", textContentC, btnC, question));
        //btnD.onClick.AddListener(() => HandleOnclickButtonOption("D", textContentD, btnD, question));


        


        answer = question.answer; // khúc ni bot tự động set câu hỏi luôn hoặc tính toán gì đó
        percenRandom.Add(answer);
        boxRandom.Add(question.options[0]);
        boxRandom.Add(question.options[1]);
        boxRandom.Add(question.options[2]);
        boxRandom.Add(question.options[3]);


        StartCoroutine(ExampleCoroutine());

        
    }
    public void HandleOnclickButtonOption(string option, Text content, Button btnRef, QuestionModel<string> question)
    {
        Debug.Log(question.answer);
        if (btnRef.image.sprite != choiseOption)
        {
            resetOpption(btnRef);
            answer = content.text;
        }




        // so sanh
        if (question.answer == content.text)
        {
            Debug.Log("Ban tra loi dung");
            managerPointAndTime = FindAnyObjectByType<ManagerPointAndTime>();
            managerPoint = FindAnyObjectByType<ManagerPointAndTime>();
            managerPointAndTime.ResetTime();
            managerPoint.addPoint();
        }
    }

    
        

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(2);
        for (var i = 0; i < boxRandom.Count; i++)
        {
            if (boxRandom[i] == answer)
            {
                boxRandom.RemoveAt(i);
            }
        }

        if (botSmart == 0)
        {
            botAnswer = answer;
        }
        else
        {
            Random random = new Random();
            for (int i = 0; i < botSmart; i++)
            {
                int randomIndex = random.Next(boxRandom.Count);
                percenRandom.Add(boxRandom[randomIndex]);
                boxRandom.Remove(boxRandom[randomIndex]);

            }

            int botRandomIndex = random.Next(percenRandom.Count);
            botAnswer = percenRandom[botRandomIndex];
        }



        if (botAnswer == textContentA.text)
        {
            resetOpption(btnA);
        }

        if (botAnswer == textContentB.text)
        {
            resetOpption(btnB);
        }

        if (botAnswer == textContentC.text)
        {
            resetOpption(btnC);
        }

        if (botAnswer == textContentD.text)
        {
            resetOpption(btnD);
        }
    }

    void resetOpption(Button excludeOption = null)
    {

        btnA.image.sprite = currentOption;
        btnB.image.sprite = currentOption;
        btnC.image.sprite = currentOption;
        btnD.image.sprite = currentOption;

        if (excludeOption != null)
        {
            excludeOption.image.sprite = choiseOption;
        }
    }

    private void loadQuestionCurrenr(QuestionModel<string> question)
    {

        answer = "";
        QuetionIndeces.text = "Câu hoi";
        contentQuestion.text = question.content.des;

        textContentA.text = question.options[0];
        textContentB.text = question.options[1];
        textContentC.text = question.options[2];
        textContentD.text = question.options[3];

    }
    public bool isFinish()
    {
        return finish;
    }
}
