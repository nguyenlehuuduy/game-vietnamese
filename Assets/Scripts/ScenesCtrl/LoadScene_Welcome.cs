using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene_Welcome : MonoBehaviour
{
    private int click_duaxe = 1;
    public void LoadToWelcome()
    {
        SceneManager.LoadScene("Welcome");

    }
    public void LoadToLogin()
    {
        SceneManager.LoadScene("Login_Server");
        
    }
    public void LoadToRegister()
    {
        SceneManager.LoadScene("Register_Server");
    }

    public void LoadToForgetPass()
    {
        SceneManager.LoadScene("ForgetPass");
    }

    public void LoadToHome()
    {
        SceneManager.LoadScene("Home");
    }

    public void LoadToHomeMap()
    {
        SceneManager.LoadScene("Home-map");
    }
    public void LoadToLearn()
    {
        SceneManager.LoadScene("Learn");
    }
    public void LoadToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void LoadToPlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level");
    }

    public void LoadToGameTypeDragDrop()
    {
        SceneManager.LoadScene("questionTypeDragDrop");
    }

    public void LoadtoChallange()
    {
        SceneManager.LoadScene("Challenge_Image_and_Multichoice");
        Debug.Log("Bé Bảo nè");
    }

    public void LoadToGameTypeMultichoiceButton()
    {
        if(click_duaxe > 1) { 
    
            SceneManager.LoadScene("Challenge_Image_and_Multichoice");

        }
        else
        {
            click_duaxe++;
        }
    }
}
