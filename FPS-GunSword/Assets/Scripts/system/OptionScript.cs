using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionScript : MonoBehaviour
{

    public bool IsOption { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            IsOption = !IsOption;

            if (IsOption)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    // タイムスケール使用時用に
    private void PauseGame()
    {
        
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
