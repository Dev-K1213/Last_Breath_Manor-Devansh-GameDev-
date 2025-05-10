using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    // Start is called before the first frame update

    public Text text;
    float timer = 5f;

    int num = 0; 

    void Start()
    {
        text.text = "You were out hiking in the woods and got lost and saw this cabin...";
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f && num == 0){
            nextText();
            timer = 5f;
            num++;
        }

        if(timer <= 0.0f && num == 1){
            nextText2();
            timer = 5f;
            num++;
        }

        if(timer <= 0.0f && num == 2){
            nextText3();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void nextText(){

        text.text = "You were getting tired and hungry so you decided to enter...";
    }

    void nextText2(){

        text.text = "The door closes behind you and locks and now you're trapped...";
    }

    void nextText3(){

        text.text = "Now you must escape or DIE trying...";
    }
}
