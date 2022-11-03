using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private bool changeMode;
    // Start is called before the first frame update
    void Start()
    {
        PlayerShotScript.instance.Start();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDefaultMove.instance.Update();

        //モード切替
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(changeMode)
            {
                changeMode = false;
            }
            else
            {
                changeMode = true;
            }
        }

        if(changeMode)  //モード変更
        {
            PlayerShotScript.instance.Update();
        }
        else
        {
            PlayerSlashScript.instance.Update();
        }
        
    }
}
