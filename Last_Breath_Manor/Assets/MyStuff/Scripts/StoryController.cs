using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    // Start is called before the first frame update

    public Text text;
    float timer = 7f;

    int num = 0; 

    void Start()
    {
        text.text = "You were out hiking in the woods and got lost and saw this cabin...";
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f){
            nextText();
        }    
    }

    void nextText(){

        text.text = "You were getting tired and hungry so you decided to enter...";
    }
}
