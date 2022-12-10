using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public SavePoint instance;
    public int stageNum;
    public static bool[] saveStage = new bool[3];
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
       for(int i =0;i<saveStage.Length;i++)
        {
            saveStage[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.transform.position,this.transform.position)<30)
        {
            Debug.Log("clear");
            saveStage[stageNum] = true;
        }
    }

}
