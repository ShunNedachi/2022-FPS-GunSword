using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    int currentStage;
    int remainEnemy;
    int playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(remainEnemy<=0)
        {
            GameClear();
        }

        if(playerHealth<=0)
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
