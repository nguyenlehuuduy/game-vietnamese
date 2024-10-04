using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;


[Serializable]
public class Bot_GameMulWithImage : MonoBehaviour
{
    // for UI
    public List<Button> btnOptions;
    public List<Image> imagesToLoad;
    private List<string> options = new List<string>();
    private List<string> imagePaths = new List<string>();
    public List<string> answers = new List<string>();
    public TextMeshProUGUI desciption = new TextMeshProUGUI();
    //for logic game
    public List<TextMeshProUGUI> textMeshProList = new List<TextMeshProUGUI>();
    public List<(string path, string answer)> pathAnswerPairs = new List<(string, string)>();
    public ManagerPointAndTime managerPoint;
    public List<GameObject> answerPanel;
    public bool right = true;
    public int percentRandom = 40;

    void Start()
    {
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
            StartCoroutine(SequentialMoveOptionsToAnswers());
        }
        else
        {
            Debug.Log(1);
        }
    }
    private IEnumerator SequentialMoveOptionsToAnswers()
    {
        string optionMove = "";
        string answerMove = "";
        int index = 0;
        
        int randomNumber = UnityEngine.Random.Range(0, 100);

        if (randomNumber < percentRandom)
        {
            for (int i = 0; i < btnOptions.Count; i++)
            {
                optionMove = btnOptions[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text.Trim().ToLower();
                answerMove = answerPanel[index].gameObject.GetComponentInChildren<TextMeshProUGUI>().text.Trim().ToLower();
                while (optionMove != answerMove)
                {
                    index++;
                    answerMove = answerPanel[index].gameObject.GetComponentInChildren<TextMeshProUGUI>().text.Trim().ToLower();
                    if (index == 3)
                    {
                        index = 0;
                    }
                }
                yield return StartCoroutine(SmoothMove(btnOptions[i].gameObject, answerPanel[index]));
                index = 0;
            }
        }
        else
        {
           
            for (int i = 0; i < btnOptions.Count; i++)
            {
                optionMove = btnOptions[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text.Trim().ToLower();
                answerMove = answerPanel[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text.Trim().ToLower();
                if (optionMove != answerMove)
                {
                    right = false;
                }
                yield return StartCoroutine(SmoothMove(btnOptions[i].gameObject, answerPanel[i]));
            }
        }
        if (right)
        {
            Debug.Log("true");
        }
        else
        {
            Debug.Log("false");
        }
           
    }

    private IEnumerator SmoothMove(GameObject startObject, GameObject targetObject)
    {
        yield return new WaitForSeconds(1f); // Chờ 1 giây giữa các di chuyển

        float elapsedTime = 0f;
        Vector3 startingPos = startObject.transform.position;
        Vector3 targetPos = targetObject.transform.position;
        float moveSpeed = 1.5f;
        while (elapsedTime < 1f)
        {
            startObject.transform.position = Vector3.Lerp(startingPos, targetPos, elapsedTime);
            elapsedTime += Time.deltaTime * (moveSpeed+0.5f);
            yield return null;
        }

        startObject.transform.position = targetPos;
    }

    public void setDes(QuestionModel<string> question)
    {
         desciption.text = question.content.des;
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
        
        return true;
    }
}