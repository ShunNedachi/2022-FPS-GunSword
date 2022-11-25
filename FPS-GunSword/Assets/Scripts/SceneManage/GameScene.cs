using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    public Text TextFrame;
    private float elapsedTime;
    int currentStage;
    int remainEnemy;
    int playerHealth;
    [SerializeField] private int timeLimit = 180;
    [SerializeField] private int graceTime = 5;
    bool timeOver;
    int graceTimeCounter;
   public static float clearTime;

    // Start is called before the first frame update
    void Start()
    {
        timeOver = false ;
        graceTimeCounter = 0;
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        float currentTime = timeLimit - elapsedTime;
        int min =(int)currentTime/60;
        int sec = (int)currentTime-(min * 60);
        if (sec / 10 >= 1)
        {
            TextFrame.text = string.Format("{0}:{1}", min, sec);
        }
        else
        {
            TextFrame.text = string.Format("{0}:0{1}", min, sec);
        }



        if(timeLimit<0)
        {
            timeOver = true;
        }
        if(timeOver)
        {
            graceTimeCounter++;
        }
        clearTime++;
        //if(remainEnemy<=0)
        //{
        //    GameClear();
        //}

        //if(playerHealth<=0||graceTime<graceTimeCounter)
        //{
        //    GameOver();
        //}
    }

    void GameClear()
    {
        SceneManager.LoadScene("ClearScene");
    }
    void GameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }

}
