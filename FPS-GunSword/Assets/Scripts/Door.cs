using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    public int stageNum;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if(GameScene.instance.stageClear[stageNum])
        {
            Destroy(this);
        }
    }
}
