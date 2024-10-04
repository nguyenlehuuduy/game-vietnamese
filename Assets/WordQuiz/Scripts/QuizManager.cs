using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;
    [SerializeField]
    private QuestionData question;
    [SerializeField]
    private Image questionImage;
    [SerializeField]
    private WordData[] answerWordArray;
    [SerializeField]
    private WordData[] optionsWordArray;
    public ManagerPointAndTime managerPointAndTime;

    private string[] charArray = new string[12] ;
    private int currentAnswerIndex = 0;
    private bool correctAnswer = true;

    private List<int> selectedWordIndex;

    private int currentQuestionIndex = 0;


    private void Awake()
    {
        if (instance == null) instance = this;
        else
            Destroy(gameObject);
        selectedWordIndex = new List<int>();
    }

    public void play(QuestionModel<string> qs)
    {
        managerPointAndTime = FindAnyObjectByType<ManagerPointAndTime>();
        SetQuestion(qs);
    }


    private void SetQuestion(QuestionModel<string> qs)
    {
        Debug.Log("Length of answer before SetQuestion: " + question.answer.Length);
        Debug.Log("Answer before SetQuestion: " + question.answer);
        currentAnswerIndex = 0;
        selectedWordIndex.Clear();

        ResetQuestion(qs);

        questionImage.sprite = Resources.Load<Sprite>(qs.content.images[0]);
        question.answer = qs.answer;

        Debug.Log(qs.answer);


        for (int i = 0; i < qs.answer.Length; i++)
        {
            charArray[i] = qs.answer[i].ToString();
            
        }
        for (int i = qs.answer.Length; i < optionsWordArray.Length; i++)
        {
          
            charArray[i] = ((char)Random.Range(97, 123)).ToString();
        }
        charArray = ShuffleList.ShuffleListItems<string>(charArray.ToList()).ToArray();
        for (int i = 0; i < optionsWordArray.Length; i++)
        {
            optionsWordArray[i].SetString(charArray[i]);
        }

        currentQuestionIndex++;

    }

    public void SelectedOption(WordData wordData)
    {
        if (currentAnswerIndex >= question.answer.Length || currentAnswerIndex >= answerWordArray.Length) return;

        selectedWordIndex.Add(wordData.transform.GetSiblingIndex());
        answerWordArray[currentAnswerIndex].SetString(wordData.charValue.ToString());
        wordData.gameObject.SetActive(false);
        currentAnswerIndex++;

        if (currentAnswerIndex >= question.answer.Length)
        {
            correctAnswer = isFinish();

            if (correctAnswer)
            {
                Debug.Log("Correct Answer");
            }
            else
            {
                Debug.Log("Incorrect Answer");
            }
        }
    }


    private void ResetQuestion(QuestionModel<string> qs)
    {
        questionImage.sprite = Resources.Load<Sprite>(qs.content.images[0]);
        question.answer = qs.answer;
        for (int i = 0; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].gameObject.SetActive(true);
            answerWordArray[i].SetString("_");
        }
        for (int i = qs.answer.Length; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].gameObject.SetActive(false);
        }

    
    }
    public void ResetLastWord()
    {
        if (selectedWordIndex.Count > 0)
        {
            int index = selectedWordIndex[selectedWordIndex.Count - 1];
            optionsWordArray[index].gameObject.SetActive(true);
            selectedWordIndex.RemoveAt(selectedWordIndex.Count - 1);

            currentAnswerIndex--;
            answerWordArray[currentAnswerIndex].SetString("_");

        }



    }
    public bool isFinish()
    {
        for (int i = 0; i < question.answer.Length; i++)
        {
            if (question.answer[i].ToString() != answerWordArray[i].charValue)
            {
                return false;
            }
        }
        managerPointAndTime = FindAnyObjectByType<ManagerPointAndTime>();
        //managerPointAndTime.addPoint();
        return false;
    }

    
}

[System.Serializable]
public class QuestionData
{
    public Sprite questionImage;
    public string answer;
}