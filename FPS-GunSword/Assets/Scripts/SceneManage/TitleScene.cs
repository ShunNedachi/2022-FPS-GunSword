using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TitleScene : MonoBehaviour
{
    

    // Start is called before the first frame update
    public Image titleImage;
    public Sprite titleSprite;
    public Image startImage;
    public Sprite startSprite;
    public Image endImage;
    public Sprite endSprite;
    void Start()
    {
        titleImage.sprite = titleSprite;
        startImage.sprite = startSprite;
        endImage.sprite = endSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
   public void ChangeScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ShutDown()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}