using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

public class BotQuizManager : MonoBehaviour
{
    public static BotQuizManager instance;
    [SerializeField]
    private QuestionBotData Botquestion; 
    [SerializeField]
    private Image BotquestionImage;
    [SerializeField]
    private WordData[] BotAnswerWordArray;
    [SerializeField]
    private WordData[] BotOptionsWordArray;

    private string[] charArray = new string[12];
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

    public void play(QuestionModel<string> Botqs)
    {
        SetQuestion(Botqs);
    }
    private void Start()
    {
        play(testQuestion());
    }
   
    private void SetQuestion(QuestionModel<string> Botqs)
    {
        Debug.Log("Length of answer before SetQuestion: " + Botquestion.answer.Length);
        Debug.Log("Answer before SetQuestion: " + Botquestion.answer);
        currentAnswerIndex = 0;
        selectedWordIndex.Clear();

        ResetQuestion(Botqs);

        BotquestionImage.sprite = Resources.Load<Sprite>(Botqs.content.images[0]);
        Botquestion.answer = Botqs.answer;

        Debug.Log(Botqs.answer);

        for (int i = 0; i < Botqs.answer.Length; i++)
        {
            charArray[i] = Botqs.answer[i].ToString();

            
            BotAnswerWordArray[i].SetString(charArray[i]);
            for (int j = 0; j < BotAnswerWordArray.Length; j++)
            {
                
                BotAnswerWordArray[i].gameObject.SetActive(true);
            }
        }

        for (int i = Botqs.answer.Length; i < BotOptionsWordArray.Length; i++)
        {
            charArray[i] = ((char)Random.Range(97, 123)).ToString();
          

        }

        charArray = BotShuffleList.ShuffleListItems<string>(charArray.ToList()).ToArray();

        for (int i = 0; i < BotOptionsWordArray.Length; i++)
        {
            BotOptionsWordArray[i].SetString(charArray[i]);
           
        }


        currentQuestionIndex++;
    }

    public void SelectedOption(WordData WordData)
    {
        if (currentAnswerIndex >= Botquestion.answer.Length || currentAnswerIndex >= BotAnswerWordArray.Length)
            return;

        selectedWordIndex.Add(WordData.transform.GetSiblingIndex());
        BotAnswerWordArray[currentAnswerIndex].SetString(WordData.charValue.ToString());
        WordData.gameObject.SetActive(false);
        currentAnswerIndex++;

        if (currentAnswerIndex >= Botquestion.answer.Length)
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

    private void ResetQuestion(QuestionModel<string> Botqs)
    {
        BotquestionImage.sprite = Resources.Load<Sprite>(Botqs.content.images[0]);
        Botquestion.answer = Botqs.answer;
        for (int i = 0; i < BotAnswerWordArray.Length; i++)
        {
            BotAnswerWordArray[i].gameObject.SetActive(true);
            BotAnswerWordArray[i].SetString("_");
        }
        for (int i = Botqs.answer.Length; i < BotAnswerWordArray.Length; i++)
        {
            BotAnswerWordArray[i].gameObject.SetActive(false);
        }


    }

    

    public bool isFinish()
    {
        for (int i = 0; i < Botquestion.answer.Length; i++)
        {
            if (Botquestion.answer[i].ToString() != BotAnswerWordArray[i].charValue)
            {
                return false;
            }
        }
        return true;
    }
    QuestionModel<string> testQuestion()
    {
        string jsonContent = File.ReadAllText("Assets/ron.json");
        List<QuestionModel<string>> Botqs = JsonConvert.DeserializeObject<List<QuestionModel<string>>>(jsonContent);
        return Botqs[0];
    }

}

[System.Serializable]
public class QuestionBotData
{
    public Sprite BotquestionImage;
    public string answer;
}
