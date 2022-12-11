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
    public Image clearImage;
    public Sprite clearSprite;
    public Image nextImage;
    public Sprite nextSprite;
    void Start()
    {
        clearImage.sprite = clearSprite;
        nextImage.sprite = nextSprite;
        clearTime = GameScene.clearTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        TextFlame.text = string.Format("{0:###.#}", clearTime);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
