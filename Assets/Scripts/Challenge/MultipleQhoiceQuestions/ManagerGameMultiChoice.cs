using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGameMultiChoice : MonoBehaviour
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
    public string answerTrue;
    private bool finish = false;
    public bool Finish
    {
        get { return finish; }
        set { finish = value; }
    }


    [SerializeField]
    public void play(QuestionModel<string> question)
    {
        loadQuestionCurrenr(question);
        btnA.onClick.AddListener(() => HandleOnclickButtonOption("A", textContentA, btnA,question));
        btnB.onClick.AddListener(() => HandleOnclickButtonOption("B", textContentB, btnB, question));
        btnC.onClick.AddListener(() => HandleOnclickButtonOption("C", textContentC, btnC, question));
        btnD.onClick.AddListener(() => HandleOnclickButtonOption("D", textContentD, btnD, question));

    }
    public void HandleOnclickButtonOption(string option , Text content , Button btnRef, QuestionModel<string> question )
    {
        if(btnRef.image.sprite != choiseOption)
        {
            resetOpption(btnRef);
        }
        answer = content.text;


        // so sanh
        managerPointAndTime = FindAnyObjectByType<ManagerPointAndTime>();
        managerPoint = FindAnyObjectByType<ManagerPointAndTime>();
        managerPointAndTime.ResetTime();

        if(answerTrue == answer)
        {
            Debug.Log("Dung");
            managerPoint.addPoint();
            //tinh diem
            //finish = true;
            StartCoroutine(ExampleCoroutine());
        }

    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(2);
        finish = true;
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

    private void loadQuestionCurrenr( QuestionModel<string> question)
    {
        answer = "";
        answerTrue = question.answer;
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
