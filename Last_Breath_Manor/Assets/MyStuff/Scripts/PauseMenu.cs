using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;
    public GameObject Tutorial;
    public GameObject hint;

    public bool isPaused;
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Block Escape key if hint or tutorial are open
        if (IsInHintOrTutorial())
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
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

    public void ShowHint()
    {

        pauseMenu.SetActive(false);

        CanvasGroup canvasGroup = hint.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

        }
            pauseMenu.SetActive(false);
            Tutorial.SetActive(false);
            hint.SetActive(true);
    }


    public void GoToMainMenu(){

        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame(){

        Application.Quit();
    }

    public void TutorialEnter()
    {

        pauseMenu.SetActive(false);

        CanvasGroup canvasGroup = Tutorial.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true; 


        }

            pauseMenu.SetActive(false);
            Tutorial.SetActive(true);
            hint.SetActive(false);
    }

    public void HideHint()
    {
        CanvasGroup canvasGroup = hint.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        Tutorial.SetActive(false);
        hint.SetActive(false);
        pauseMenu.SetActive(true);
    }


    public void TutorialExit()
    {
        CanvasGroup canvasGroup = Tutorial.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        Tutorial.SetActive(false);
        hint.SetActive(false);
        pauseMenu.SetActive(true);
    }



    public void Restart(){

        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }

    private bool IsInHintOrTutorial()
    {
        CanvasGroup hintGroup = hint.GetComponent<CanvasGroup>();
        CanvasGroup tutorialGroup = Tutorial.GetComponent<CanvasGroup>();

        return (hintGroup != null && hintGroup.interactable) || (tutorialGroup != null && tutorialGroup.interactable);

    }

}
