using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    public static GameScene instance;
    private float elapsedTime;
    int currentStage;
    int remainEnemy;
    int playerHealth;
    [SerializeField] private int timeLimit = 180;
    [SerializeField] private int graceTime = 5;
    bool timeOver;
    int graceTimeCounter;
    public static float clearTime;

    [SerializeField] public int StageNum = 0;
    [SerializeField] public int currentCoreNum = 0;
    public int currentStageNum = 0;
    public int maxStageNum = 1;
    public int[] coreMaxCount;
    private bool onlyOnce = true;
    public bool[] stageClear;
    public Text TextFrame;
    public Text coreText;
   
    // Start is called before the first frame update
    void Start()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        timeOver = false ;
        graceTimeCounter = 0;
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (onlyOnce)
        {
            coreMaxCount[0] = CoreScript.coreCount0;
            coreMaxCount[1] = CoreScript.coreCount1;
            coreMaxCount[2] = CoreScript.coreCount2;
            onlyOnce = false;
        }
        if (currentCoreNum == 0)
        {
            coreText.text = string.Format("{0}/{1}", CoreScript.coreCount0, coreMaxCount[0]);
            if (CoreScript.coreCount0 <= 0)
            {
                stageClear[currentCoreNum] = true;
                currentCoreNum++;
            }
            /*coreText.text = string.Format("{0}/{1}", CoreScript.coreCount0, coreMaxCount[0]);
            if (CoreScript.coreCount0 <= 0)
            {
                stageClear[currentStageNum] = true;
            }*/
        }
        if (currentCoreNum == 1)
        {
            coreText.text = string.Format("{0}/{1}", CoreScript.coreCount1, coreMaxCount[1]);
            if (CoreScript.coreCount1 <= 0)
            {
                stageClear[currentCoreNum] = true;
                currentCoreNum++;
            }
        }
        if (currentCoreNum == 2)
        {
            coreText.text = string.Format("{0}/{1}", CoreScript.coreCount1, coreMaxCount[2]);
            if (CoreScript.coreCount2 <= 0)
            {
                stageClear[currentCoreNum] = true;
                currentCoreNum++;
            }
        }

        elapsedTime += Time.deltaTime;
        float currentTime = timeLimit - elapsedTime;
        int min = (int)currentTime / 60;
        int sec = (int)currentTime - (min * 60);
        if (sec / 10 >= 1)
        {
            TextFrame.text = string.Format("{0}:{1}", min, sec);
        }
        else
        {
            TextFrame.text = string.Format("{0}:0{1}", min, sec);
        }



        if (timeLimit < 0)
        {
            timeOver = true;
        }
        if (timeOver)
        {
            graceTimeCounter++;
        }
        clearTime++;

        if(SavePoint.saveStage[0]==true)
        {
            GameClear();
        }

        if(PlayerHPScript.instance.GethP()<0||graceTime<graceTimeCounter)
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
