using System;

public class MeleeEnemy : DefaultEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        // 自身の周辺に近距離エネミーを3,4沸かせる
        Random rand = new System.Random();
        int spawnRandNum = rand.Next(3, 5);

        // 生成処理は後ほど追加
        for (int i = 0; i < spawnRandNum; i++)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Move()
    {
        
    }
}
