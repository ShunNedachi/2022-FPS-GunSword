using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOverScene : MonoBehaviour
{
    public Image gameOverImage;
    public Sprite gameOverSprite;
    public Image nextImage;
    public Sprite nextSprite;
    // Start is called before the first frame update
    void Start()
    {
        gameOverImage.sprite = gameOverSprite;
        nextImage.sprite = nextSprite;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
   public void ChangeScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
