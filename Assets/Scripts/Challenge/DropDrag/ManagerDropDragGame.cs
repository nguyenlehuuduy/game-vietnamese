using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;

public class ManagerDropDragGame : MonoBehaviour
{
    public ManagerPointAndTime managerPoint;
    public ManagerPointAndTime managerPointAndTime;
    public bool finish = false;


    // Start is called before the first frame update
    public GameObject[] listItemOption;
    public GameObject[] listSlot;
    public Button btnReload;
    public Button submitBtn;
    public string[] answerSubmit = {"","",""};
    public int pointInQuestion = 0;
    public GameObject cacheChild1;
    public GameObject cacheChild2;
    public GameObject cacheChild3;

    public string text1 = "";
    public string text2 = "";
    public string text3 = "";



    public string[] answer1;
    public string[] answer2;
    public string[] answer3;
    public string[] answer4;



    public void play(QuestionModel<string> qs)
    {
        
        loadQuestionCurrenrToUI(qs);

        btnReload.onClick.AddListener(() =>
        {
            
            if(cacheChild1 != null)
            {
                string name = cacheChild1.name;
                int index = Int32.Parse(name[4..].ToString());
                Debug.Log(listItemOption.Length);

                cacheChild1.transform.parent = listItemOption[index-1].transform;
            }

            if (cacheChild2 != null)
            {
                string name2 = cacheChild2.name;
                int index = Int32.Parse(name2[4..].ToString());
                Debug.Log(listItemOption.Length);

                cacheChild2.transform.parent = listItemOption[index - 1].transform;
            }

            if (cacheChild3 != null)
            {
                string name3 = cacheChild3.name;
                int index = Int32.Parse(name3[4..].ToString());
                cacheChild3.transform.parent = listItemOption[index - 1].transform;
            }
        });

        submitBtn.onClick.AddListener(() =>
        {
            Debug.Log("oke");


            if (text1  != null && text2 != null && text3!= null)
            {
                text1 = text1.ToLower().Trim();
                text2 = text2.ToLower().Trim();
                text3 = text3.ToLower().Trim();
                string[] useSubmicAnswer = { text1, text2, text3 };

                if (answer1.All(a => useSubmicAnswer.Contains(a)))
                {
                    clearAnswer();
                    Debug.Log("Chúc mừng bạn");
                    return;
                }

                if (answer2.All(a => useSubmicAnswer.Contains(a)))
                {
                    clearAnswer();
                    Debug.Log("Chúc mừng bạn");
                    return;
                }


                if (answer3.All(a => useSubmicAnswer.Contains(a)))
                {
                    clearAnswer();
                    Debug.Log("Chúc mừng bạn");
                    return;
                }

                if (answer4.All(a => useSubmicAnswer.Contains(a)))
                {
                    clearAnswer();
                    Debug.Log("Chúc mừng bạn");
                    return;
                }

                Debug.Log("Sai");
            }
            else
            {
                Debug.Log("Vui lòng chọn hết đáp án vào ô trống");
            }

            
            
        });
    }

    void clearAnswer()
    {
        foreach (GameObject item in listSlot)
        {
            if(item.transform.childCount == 1)
            {
                Destroy(item.transform.GetChild(0).gameObject);
            }
        }

        managerPointAndTime = FindAnyObjectByType<ManagerPointAndTime>();
        managerPoint = FindAnyObjectByType<ManagerPointAndTime>();
        //managerPointAndTime.ResetTime();
        managerPoint.addPoint();
        pointInQuestion += 4;

    }

    public bool isFinish()
    {
        if(pointInQuestion == 16)
        {
            return true;

        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < listItemOption.Length; i++)
        {
            GameObject option = listItemOption[i];
            if(option.transform.childCount > 0)
            {
                GameObject btn = listItemOption[i].transform.GetChild(0).gameObject;
                btn.GetComponent<DragAndDrop>().enabled = true;
            }
        }


        // xử lý lưu option người chơi kéo vào ô slot
        for (int i = 0; i < listSlot.Length; i++)
        {
            GameObject item = listSlot[i];
            
            if(item.transform.childCount > 0 )
            {
                GameObject Button = listSlot[i].transform.GetChild(0).gameObject;
                
                Text text = Button.transform.GetChild(0).gameObject.GetComponent<Text>();


                if ( i == 0)
                {
                    text1 = text.text;
                    cacheChild1 = Button;
                }

                if (i == 1)
                {
                    text2 = text.text;
                    cacheChild2 = Button;

                }

                if (i == 2)
                {
                    text3 = text.text;
                    cacheChild3 = Button;
                }
            }

        }

        // xử lý option người chơi kéo về lại item slot
        for (int i = 0; i < listSlot.Length; i++)
        {
            GameObject item = listSlot[i];

            //if (item.transform.childCount > 0)
            //{
             //   item.transform.GetChild(0).GetComponent<DragAndDrop>().enabled = false;
            //}


            if (item.transform.childCount == 0)
            {
                if (item.name == "slot1")
                {
                    cacheChild1 = null;
                    text1 = null;
                }

                if (item.name == "slot2")
                {
                    cacheChild2 = null;
                    text2 = null;
                }

                if (item.name == "slot3")
                {
                    cacheChild3 = null;
                    text3 = null;
                }
            }
        }




        
    }

    // đọc json
    JSONDataQuestionClass<string> loadJsonQuestions(string url)
    {
        string json = File.ReadAllText(UnityEngine.Application.dataPath + url);
        Debug.Log(json);

        JSONDataQuestionClass<string> data = JsonUtility.FromJson<JSONDataQuestionClass<string>>(json);
        return data;
    }



        
    void showDataToUI(QuestionModel<string> qs)
    {
        // lặp qua list ObjectOption các sự lựa chọn
        for (int i = 0; i < listItemOption.Length; i++)
        {
            GameObject Button = listItemOption[i].transform.GetChild(0).gameObject; // lấy object con
            Text text = Button.transform.GetChild(0).gameObject.GetComponent<Text>(); // lấy object text trong object con
            text.text = qs.options[i]; // setText
        }
    }


    private void loadQuestionCurrenrToUI(QuestionModel<string> qs)
    {
        
        answer1 = qs.answer.Split("|")[0].Split(",");
        answer2 = qs.answer.Split("|")[1].Split(",");
        answer3 = qs.answer.Split("|")[2].Split(",");
        answer4 = qs.answer.Split("|")[3].Split(",");


        showDataToUI(qs);
    }



}
