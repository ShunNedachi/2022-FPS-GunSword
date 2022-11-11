using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] public int deformationInterval = 30;

    private bool ULT = false;
    private int deformationTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        PlayerShotScript.instance.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            ULT = true;

        }

        PlayerDefaultMove.instance.Update();

        if(ULT)
        {
            CameraController.instance.ChangeThirdViewCamera();
            deformationTimer++;
            if(deformationTimer>deformationInterval)
            {
                //ULT中処理

            }
        }
        else
        {
            PlayerShotScript.instance.Update();
            PlayerSlashScript.instance.Update();
        } 
    }
}
