using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public void PlayGame(){

        SceneManager.LoadScene("Story");
    }

    public void QuitGame(){

        Application.Quit();
    }

    public void Tutorial(){

        SceneManager.LoadScene("Tutorial");
    }
}
