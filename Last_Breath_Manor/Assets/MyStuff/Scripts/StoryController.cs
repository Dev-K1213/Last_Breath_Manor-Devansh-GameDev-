using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    public Text text;
    public GameObject previous;
    public GameObject next;
    public GameObject play;

    private int num = 0;

    void Start()
    {
        UpdateStory();
    }

    public void nextText()
    {
        if (num < 3)
        {
            num++;
            UpdateStory();
        }
    }

    public void previousText()
    {
        if (num > 0)
        {
            num--;
            UpdateStory();
        }
    }

    public void playButton()
    {
        SceneManager.LoadScene("Main");
    }

    void UpdateStory()
    {
        
        switch (num)
        {
            case 0:
                text.text = "You were out hiking in the woods and got lost and saw this cabin...";
                break;
            case 1:
                text.text = "You were getting tired and hungry so you decided to enter...";
                break;
            case 2:
                text.text = "The door closes behind you and locks and now you're trapped...";
                break;
            case 3:
                text.text = "Now you must escape or DIE trying...";
                break;
        }

        previous.SetActive(num > 0);
        next.SetActive(num < 3);
        play.SetActive(num == 3);
    }
}
