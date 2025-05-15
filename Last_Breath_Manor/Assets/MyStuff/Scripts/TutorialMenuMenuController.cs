using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMenuMenuController : MonoBehaviour
{
    public void BackToMenu(){

        SceneManager.LoadScene("Menu");
    }
}
