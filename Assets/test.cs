using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string jsonData = PlayerPrefs.GetString("users");
        Debug.Log("jsonData: " + jsonData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
