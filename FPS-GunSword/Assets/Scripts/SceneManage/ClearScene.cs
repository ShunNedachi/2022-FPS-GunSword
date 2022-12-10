using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ClearScene : MonoBehaviour
{
    // Start is called before the first frame update
    float clearTime;
    public Text TextFlame;
    void Start()
    {
        clearTime = GameScene.clearTime;
        Invoke("ChangeScene", 1.5f);

    }

    // Update is called once per frame
    void Update()
    {
        TextFlame.text = string.Format("{0:###.#}", clearTime);
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
