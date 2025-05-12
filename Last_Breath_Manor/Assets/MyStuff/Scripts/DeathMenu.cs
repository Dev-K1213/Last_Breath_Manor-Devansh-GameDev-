using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void PlayGame(){

        SceneManager.LoadScene("Main");
    }

    public void QuitGame(){

        Application.Quit();
    }

    public void BackToMenu(){

        SceneManager.LoadScene("Menu");
    }
}
