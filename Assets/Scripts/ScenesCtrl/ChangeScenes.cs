using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class control : MonoBehaviour
{
    public void MoveToScene(int senceId )
    {
        SceneManager.LoadScene(senceId);
        Debug.Log("Test push");
    }
}



