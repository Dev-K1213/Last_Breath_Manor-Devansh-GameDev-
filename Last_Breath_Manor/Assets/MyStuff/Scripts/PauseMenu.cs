using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;
    public bool isPaused;
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){

            if(isPaused){
                ResumeGame();
            }
            else{

                PauseGame();
            }
        }
    }

    public void PauseGame(){

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
        
    }

    public void ResumeGame(){

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        
    }

    public void GoToMainMenu(){

        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame(){

        Application.Quit();
    }

    public void Restart(){

        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }
}
