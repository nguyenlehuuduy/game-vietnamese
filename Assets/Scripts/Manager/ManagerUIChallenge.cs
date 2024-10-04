using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ManagerUIChallenge : MonoBehaviour
{
    public List<Image> Star;
    public List<GameObject> challenges = new List<GameObject>();
    public List<GameObject> botParts = new List<GameObject>();
    public GameObject showScorePanel;
    public GameObject showLosePanel;
    public GameObject backgr;
    public ManagerPointAndTime managerPointAndTime;
    public ManagerGamePlay manager;
    private QuestionModel<string> question;
    private ManagerGameMulWithImage gameMulWithImage;
    private Bot_GameMulWithImage bot_image;
    private ManagerGameMultiChoice gameMultiChoice;
    private Bot_multichoice bot_multi;
    private ManagerDropDragGame gameDropDragGame;
    private Bot_ManagerDropDragGame bot_ManagerDropDragGame;
    private QuizManager wordQuiz;
    private bool isUpdate = true;
    private static int numberOfQuestion = 0;
    const int TOTALQUESTION = 6; // có vấn đề thằng nào code dòng
    public int point1Star = 10; 
    public int point2Star= 50;
    public int point3Star = 100;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindAnyObjectByType<ManagerGamePlay>();
        gameMulWithImage = FindAnyObjectByType<ManagerGameMulWithImage>();
        gameMultiChoice = FindAnyObjectByType<ManagerGameMultiChoice>();
        managerPointAndTime = FindAnyObjectByType<ManagerPointAndTime>();
        gameDropDragGame = FindAnyObjectByType<ManagerDropDragGame>();
        wordQuiz = FindAnyObjectByType<QuizManager>();
        bot_image = FindAnyObjectByType<Bot_GameMulWithImage>();
        bot_multi = FindAnyObjectByType<Bot_multichoice>();
        bot_ManagerDropDragGame = FindAnyObjectByType<Bot_ManagerDropDragGame>();
        showScorePanel.SetActive(false);
        showLosePanel.SetActive(false);
    }


    // Update is called once per frame
    private void Update()
    {
        if (isUpdate)
        {
            CustomUpdate();
        }
        else
        {
            if (managerPointAndTime.TimeLeft < 1)
            {
                resetQuestion();
                resetPanel();   
            }
            if (gameMulWithImage.isFinish() || gameMultiChoice.isFinish() || gameDropDragGame.isFinish() || wordQuiz.isFinish())
            {
                gameMulWithImage.Finish = false;
                gameMultiChoice.Finish = false;
                if (numberOfQuestion <= TOTALQUESTION)
                {
                    resetQuestion();
                    resetPanel();
                }
            }
        }
       
    }
    private void CustomUpdate()
    {
        hiddenPart(challenges);
        hiddenPart(botParts);
        if (question != null)
        {
            if(numberOfQuestion == TOTALQUESTION)
            {
                int point = managerPointAndTime.Point;
                showScorePanel.GetComponentInChildren<TextMeshProUGUI>().text = point.ToString();
                if(point < point1Star)
                {
                    showLosePanel.SetActive(true);
                }
                else
                {
                    if (point < point2Star)
                    {
                        for (int i = 0; i <= 1; i++)
                        {
                            Color color = Star[i + 1].color;
                            color.a = 0;
                            Star[i + 1].color = color;
                        }
                    }
                    else if (point < point3Star)
                    {
                        Color b = Star[2].color;
                        b.a = 0;
                        Star[2].color = b;
                    }
                    showScorePanel.SetActive(true);
                }
               
            }
            else
            {
                numberOfQuestion++;
                challenges[question.type.typeId - 1].SetActive(true);
                botParts[question.type.typeId - 1].SetActive(true);
                switch (question.type.typeId)
                {
                    case 1: // challenge drag drog
                        gameDropDragGame.play(question);
                        bot_ManagerDropDragGame.play(question);
                        break;
                    case 2: // challenge game with image
                        gameMulWithImage.play(question);
                        bot_image.play(question);
                        break;
                    case 3: // challenge multichoice
                        gameMultiChoice.play(question);
                        bot_multi.play(question);
                        break;
                    case 4: // challenge dien tu cua ron
                        wordQuiz.play(question);
                        break;
                    default:
                        // code block
                        break;
                }
                isUpdate = false;
            }
        }
        else
        {
            question = manager.getQuestion();
            isUpdate = true;
        }
    }
    
  
    private void hiddenPart(List<GameObject> parts)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].SetActive(false);
        }
    }
    private void resetQuestion()
    {
        question = manager.getQuestion();
        isUpdate = true;
    }
    public void resetPanel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   
}