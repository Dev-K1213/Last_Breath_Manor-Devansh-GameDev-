using UnityEngine;
using TMPro;

public class ComputerTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject timerCanvas;
    
    public float timerDuration = 300f; // 5 minutes
    public DollFollow dollFollow;

    private float timer;
    private bool timerStarted = true;

    void Start()
    {
        timer = timerDuration;
        //if (timerCanvas != null)
         //   timerCanvas.SetActive(false);
    }

    void Update()
{
    if (timerStarted && timer > 0f)
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 0f;

            // Trigger game over from DollFollow
            if (dollFollow != null)
            {
                StartCoroutine(dollFollow.GameOverJumpscare());

            }
        }

        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds) + " remaining to escape!";
    }
}


public void ShowTimer()
{
    Debug.Log("ShowTimer called");

    if (timerCanvas != null && !timerCanvas.activeSelf)
    {
        Debug.Log("Activating timer canvas");
        timerCanvas.SetActive(true);
        timerStarted = true;
    }
}

}
