using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageShowScore : MonoBehaviour
{
    public Button btnPlayAgain;
    public Button btnHome;
    public Button btnSetting;
    public Text scoreText;
    public ManagerPointAndTime managerPointAndTime;

    // Start is called before the first frame update
    void Start()
    {
        managerPointAndTime = FindAnyObjectByType<ManagerPointAndTime>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
