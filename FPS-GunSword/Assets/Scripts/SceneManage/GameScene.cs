using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        timeLimit--;
        if(timeLimit<0)
        {
            timeOver = true;
        }
        if(timeOver)
        {
            graceTimeCounter++;
        }
        clearTime++;
        if(remainEnemy<=0)
        {
            GameClear();
        }

        if(playerHealth<=0||graceTime<graceTimeCounter)
        {
            GameOver();
        }
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
